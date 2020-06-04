using StigChecklistCli.Helpers;
using StigChecklistCli.Options;
using StigChecklistCli.Schemas.v209;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StigChecklistCli.Commands
{
    /// <summary>
    /// Given a source and target STIG checklist file paths as arguments, it will copy the following from source and master/target into a "copied" file.    
    /// <list type="bullet">
    /// <item>source.vulnerability.status</item>
    /// <item>source.vulnerability.comments</item>
    /// <item>soure.vulnerability.finding_details</item>
    /// </list>    
    ///  Target file not touched, it will generate a new file.
    /// </summary>
    public class CopyCommand
    {
        private readonly CopyOptions opts;
        private string sourcePath;
        private string targetPath;

        public CopyCommand(CopyOptions opts)
        {
            this.opts = opts ?? throw new ArgumentNullException(nameof(opts), $"{nameof(CopyOptions)} object is required.");
        }

        public int Run()
        {
            int returnValue = 0;

            sourcePath = opts.SourceFilePath;
            targetPath = opts.TargetFilePath;

            var overrideTarget = opts.Override;
            var controlFilter = opts.Filter;            

            Copy(overrideTarget, controlFilter);

            return returnValue;
        }

        /// <summary>
        /// Given source and <see cref="targetPath"/> files, it will copy and generate a new file
        /// </summary>
        /// <param name="sourceFile">file containing control findings</param>
        /// <param name="targetFile">file to copy content from <paramref name="sourceFile"/></param>
        /// <param name="overrideTarget">forces override of existing data in STIG</param>
        /// <param name="controlFilter">if provided, it will filter STIGs to copy based on control</param>        
        private bool Copy(bool overrideTarget = false, string controlFilter = null)
        {
            Console.WriteLine("Starting Copy....");

            try
            {
                IDictionary<string, string> cciIds = null;

                if (!string.IsNullOrEmpty(controlFilter))
                {
                    Console.WriteLine($"Filtering STIGs by '{controlFilter}'.");

                    cciIds = StigChecklistHelper.GetCciIds(controlFilter);
                }

                var sourceChecklist = StigChecklistHelper.GetChecklist(sourcePath);
                var targetChecklist = StigChecklistHelper.GetChecklist(targetPath);

                CopyCheckList(sourceChecklist, targetChecklist, overrideTarget, cciIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Copy Failed: {ex.Message}");
                return false;
            }

            Console.WriteLine("Copy Succeeded");

            return true;
        }

        /// <summary>
        /// copies values from source to target
        /// </summary>
        /// <param name="sourceChecklist">values to copy from</param>
        /// <param name="targetChecklist">values to copy to</param>        
        private bool CopyCheckList(CHECKLIST sourceChecklist, CHECKLIST targetChecklist, bool overrideTarget = false, IDictionary<string, string> cciLookup = null)
        {
            Console.WriteLine("Building STIG dictionary....");

            var sourceStigLookup = StigChecklistHelper.GetStigLookup(sourceChecklist);
            var targetStigLookup = StigChecklistHelper.GetStigLookup(targetChecklist);

            foreach (var sourceLookup in sourceStigLookup)
            {
                if (!targetStigLookup.ContainsKey(sourceLookup.Key))
                {
                    Console.WriteLine($"Target Checklist does NOT have STIG '{sourceLookup.Key}', skipping....");
                    continue;
                }

                var targetStig = targetStigLookup[sourceLookup.Key];

                var sourceVulnerabilityLookup = StigChecklistHelper.GetVulnerabilityLookup(sourceLookup.Value.VULN);
                var targetVulnerabilityLookup = StigChecklistHelper.GetVulnerabilityLookup(targetStig.VULN);

                Console.WriteLine($"Copying '{sourceLookup.Key}' STIGs....");

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

                    CopyVulnerability(vulnerabilityLookup.Value, targetVulnerability, overrideTarget);

                    targetVulnerabilityLookup[vulnerabilityLookup.Key] = targetVulnerability;
                }

                targetStigLookup[sourceLookup.Key].VULN = targetVulnerabilityLookup.Values.ToArray();
            }

            targetChecklist.STIGS = targetStigLookup.Values.ToArray();

            StigChecklistHelper.SaveChecklist(targetChecklist, targetPath, "copied");

            return true;
        }

        /// <summary>
        /// Copies values from soure to target object
        /// </summary>
        /// <param name="source">updated values</param>
        /// <param name="target">orginal values</param>
        private static void CopyVulnerability(CHECKLISTISTIGVULN source, CHECKLISTISTIGVULN target, bool overrideTarget = false)
        {
            // value of vulnerability not reviewed, if status is this, then skip copy
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
    }
}
