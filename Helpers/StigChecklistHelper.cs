using StigChecklistUtil.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace StigChecklistUtil.Helpers
{
    /// <summary>
    /// Given a source and target STIG checklist file paths as arguments, the executable will merge (copy) the following from source and master/target into a "merged" file.    
    /// <list type="bullet">
    /// <item>source.vulnerability.status</item>
    /// <item>source.vulnerability.comments</item>
    /// <item>soure.vulnerability.finding_details</item>
    /// </list>    
    ///  Target file not touched, it will generate a new file.
    /// </summary>
    public class StigChecklistHelper
    {
        private readonly string sourcePath;
        private readonly string targetPath;

        public StigChecklistHelper(string sourcePath, string targetPath)
        {
            this.sourcePath = sourcePath;
            this.targetPath = targetPath;
        }

        /// <summary>
        /// Given source and master files, it will merge and generated a new file
        /// </summary>
        /// <param name="overrideTarget">forces override of existing data in STIG</param>
        /// <param name="controlFilter">if provided, it will filter STIGs to merge based on control</param>        
        public bool Merge(bool overrideTarget = false, string controlFilter = null)
        {
            Console.WriteLine("Starting Merge....");

            try
            {
                IDictionary<string, string> cciIds = null;

                if (!string.IsNullOrEmpty(controlFilter))
                {
                    Console.WriteLine($"Filtering STIGs by '{controlFilter}'.");

                    cciIds = GetCciIds(controlFilter);
                }

                var sourceChecklist = GetChecklist(sourcePath);
                var targetChecklist = GetChecklist(targetPath);

                MergeCheckList(sourceChecklist, targetChecklist, overrideTarget, cciIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Merge Failed: {ex.Message}");
                return false;
            }

            Console.WriteLine("Merge Succeeded");

            return true;
        }

        /// <summary>
        /// Deserializes cci list from embedded resource
        /// </summary>        
        private static cci_list GetCcilist()
        {
            Console.WriteLine($"Reading CCI list File....");

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream($"{(nameof(StigChecklistUtil))}.U_CCI_List.xml");

            var serializer = new XmlSerializer(typeof(cci_list));
            var cciList = (cci_list)serializer.Deserialize(stream);

            Console.WriteLine($"Read {cciList.cci_items.Length} CCIs.");

            return cciList;
        }

        /// <summary>
        /// Get dictionary of CCIs by <paramref name="filter"/>
        /// </summary>
        /// <param name="filter">contains filter to trim CCI list</param>
        /// <returns></returns>
        private static IDictionary<string, string> GetCciIds(string filter)
        {
            var cciList = GetCcilist();
            var cciIds = cciList.cci_items.Where(i => i.references.Any(r => r.index.Contains(filter))).Select(i => i.id).Distinct().ToList();

            Console.WriteLine($"Filtered to {cciIds.Count} CCIs by '{filter}'.");

            return cciIds.ToDictionary(c => c);
        }

        /// <summary>
        /// Deserializes stig checklist from ckl file
        /// </summary>
        /// <param name="filePath">path to ckl file</param>
        private static CHECKLIST GetChecklist(string filePath)
        {
            Console.WriteLine($"Reading STIG File '{filePath}'....");

            var serializer = new XmlSerializer(typeof(CHECKLIST));
            var sourceFileStream = new FileStream(filePath, FileMode.Open);
            var checklist = (CHECKLIST)serializer.Deserialize(sourceFileStream);

            return checklist;
        }

        /// <summary>
        /// copies values from source to target
        /// </summary>
        /// <param name="sourceChecklist">values to copy from</param>
        /// <param name="targetChecklist">values to copy to</param>        
        private bool MergeCheckList(CHECKLIST sourceChecklist, CHECKLIST targetChecklist, bool overrideTarget = false, IDictionary<string, string> cciLookup = null)
        {
            Console.WriteLine("Building STIG dictionary....");

            var sourceStigLookup = GetStigLookup(sourceChecklist);
            var targetStigLookup = GetStigLookup(targetChecklist);

            foreach (var sourceLookup in sourceStigLookup)
            {
                if (!targetStigLookup.ContainsKey(sourceLookup.Key))
                {
                    Console.WriteLine($"Master Checklist does NOT have STIG '{sourceLookup.Key}', skipping....");
                    continue;
                }

                var targetStig = targetStigLookup[sourceLookup.Key];

                var sourceVulnerabilityLookup = GetVulnerabilityLookup(sourceLookup.Value.VULN);
                var targetVulnerabilityLookup = GetVulnerabilityLookup(targetStig.VULN);

                Console.WriteLine($"Merging '{sourceLookup.Key}' STIGs....");

                foreach (var vulnerabilityLookup in sourceVulnerabilityLookup)
                {
                    var targetVulnerability = targetVulnerabilityLookup[vulnerabilityLookup.Key];

                    // if control filter was provided, check STIG CCI is in list
                    if (cciLookup != null && cciLookup.Count > 0)
                    {
                        var stigCcis = vulnerabilityLookup.Key.CiiIds;
                        var found = stigCcis.Any(s => cciLookup.ContainsKey(s));

                        if (!found)
                        {
                            // if not found, skip STIG
                            continue;
                        }
                    }

                    MergeVulnerability(vulnerabilityLookup.Value, targetVulnerability, overrideTarget);

                    targetVulnerabilityLookup[vulnerabilityLookup.Key] = targetVulnerability;
                }

                targetStigLookup[sourceLookup.Key].VULN = targetVulnerabilityLookup.Values.ToArray();
            }

            targetChecklist.STIGS = targetStigLookup.Values.ToArray();

            SaveChecklist(targetChecklist);

            return true;
        }

        /// <summary>
        /// Build STIG hash table to avoid redundant loops
        /// </summary>
        /// <param name="checklist"></param>        
        private static IDictionary<string, CHECKLISTISTIG> GetStigLookup(CHECKLIST checklist)
        {
            var dictionary = new Dictionary<string, CHECKLISTISTIG>();
            var stigs = checklist.STIGS.ToList();

            foreach (var stig in stigs)
            {
                var stidId = stig.STIG_INFO.Where(i => i.SID_NAME == "stigid").Select(i => i.SID_DATA).Single();

                dictionary.Add(stidId, stig);
            }

            return dictionary;
        }

        /// <summary>
        /// Serialize checklist object back to XML, saves to new file
        /// </summary>
        /// <param name="checklist">file to serialize</param>
        private void SaveChecklist(CHECKLIST checklist)
        {
            Console.WriteLine("Saving Merged Checklist....");

            var serializer = new XmlSerializer(typeof(CHECKLIST));
            var mergedFile = targetPath.Replace(".ckl", ".merged.ckl");
            var file = File.Create(mergedFile);

            serializer.Serialize(file, checklist);

            file.Close();
        }

        /// <summary>
        /// Copies values from soure to target object
        /// </summary>
        /// <param name="source">updated values</param>
        /// <param name="target">orginal values</param>
        private static void MergeVulnerability(CHECKLISTISTIGVULN source, CHECKLISTISTIGVULN target, bool overrideTarget = false)
        {
            // value of vulnerability not reviewed, if status is this, then skip merge
            const string NotReviewed = "Not_Reviewed";

            // type is object when "newed" and not set
            const string ObjectType = "Object";

            if (!source.STATUS.Equals(NotReviewed) && (target.STATUS.Equals(NotReviewed) || overrideTarget))
            {
                target.STATUS = source.STATUS;

                if (!source.FINDING_DETAILS.GetType().Name.Equals(ObjectType) && (target.FINDING_DETAILS.GetType().Name.Equals(ObjectType) || overrideTarget))
                {
                    target.FINDING_DETAILS = source.FINDING_DETAILS;
                }

                if (!source.COMMENTS.GetType().Name.Equals(ObjectType) && (target.COMMENTS.GetType().Name.Equals(ObjectType) || overrideTarget))
                {
                    target.COMMENTS = source.COMMENTS;
                }
            }
        }

        /// <summary>
        /// Build Vulnerability hash table to avoid redundant loops
        /// </summary>
        /// <param name="vulnerabilitiesArray">array of vulnerabilities for STIG</param>
        /// <returns></returns>
        private static IDictionary<StigKey, CHECKLISTISTIGVULN> GetVulnerabilityLookup(CHECKLISTISTIGVULN[] vulnerabilitiesArray)
        {
            var dictionary = new Dictionary<StigKey, CHECKLISTISTIGVULN>();
            var vulnerabilities = vulnerabilitiesArray.ToList();

            foreach (var vulnerability in vulnerabilities)
            {
                var vulnerabilityId = vulnerability.STIG_DATA.ToList().Where(data => data.VULN_ATTRIBUTE == "Vuln_Num").Select(data => data.ATTRIBUTE_DATA).Single();
                var ciiIds = vulnerability.STIG_DATA.ToList().Where(data => data.VULN_ATTRIBUTE == "CCI_REF").Select(data => data.ATTRIBUTE_DATA).ToList();

                dictionary.Add(new StigKey { VulnerabilityNumber = vulnerabilityId, CiiIds = ciiIds }, vulnerability);
            }

            return dictionary;
        }
    }
}
