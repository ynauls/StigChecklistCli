using StigChecklistCli.Models;
using StigChecklistCli.Schemas.v209;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace StigChecklistCli.Helpers
{
    /// <summary>
    /// Helper method to get STIG Check list 
    /// </summary>
    public static class StigChecklistHelper
    {
        /// <summary>
        /// Deserializes cci list from embedded resource
        /// NOTE: U_CCI_List.xml was pulled from STIG Viewer 2.09
        /// </summary>        
        public static cci_list GetCcilist()
        {
            Console.WriteLine($"Reading CCI list File....");
            
            var assembly = Assembly.GetExecutingAssembly();

            string[] names = assembly.GetManifestResourceNames();
            var resourceName = $"{nameof(StigChecklistCli)}.{nameof(StigChecklistCli.Schemas)}.{nameof(StigChecklistCli.Schemas.v209)}.U_CCI_List.xml";
            var stream = assembly.GetManifestResourceStream(resourceName);

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
        public static IDictionary<string, string> GetCciIds(string filter)
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
        public static CHECKLIST GetChecklist(string filePath)
        {
            Console.WriteLine($"Reading STIG File '{filePath}'....");

            var serializer = new XmlSerializer(typeof(CHECKLIST));

            using var sourceFileStream = new FileStream(filePath, FileMode.Open);
            
            var checklist = (CHECKLIST)serializer.Deserialize(sourceFileStream);

            return checklist;
        }       

        /// <summary>
        /// Build STIG hash table to avoid redundant loops
        /// </summary>
        /// <param name="checklist"></param>        
        public static IDictionary<string, CHECKLISTISTIG> GetStigLookup(CHECKLIST checklist)
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
        /// <param name="postfix">to append to result file</param>
        public static void SaveChecklist(CHECKLIST checklist, string targetPath, string postfix)
        {
            Console.WriteLine("Saving Copied Checklist....");

            var serializer = new XmlSerializer(typeof(CHECKLIST));
            var copiedFile = targetPath.Replace(".ckl", $".{postfix}.ckl");
            var file = File.Create(copiedFile);

            serializer.Serialize(file, checklist);

            file.Close();
        }       

        /// <summary>
        /// Build Vulnerability hash table to avoid redundant loops
        /// </summary>
        /// <param name="vulnerabilitiesArray">array of vulnerabilities for STIG</param>
        /// <returns></returns>
        public static IDictionary<StigKey, CHECKLISTISTIGVULN> GetVulnerabilityLookup(CHECKLISTISTIGVULN[] vulnerabilitiesArray)
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
