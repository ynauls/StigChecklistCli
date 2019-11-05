using CommandLine;
using StigChecklistUtil.Commands;
using StigChecklistUtil.Options;
using System.Collections.Generic;

namespace StigChecklistUtil
{
    /// <summary>
    /// Given a source and target STIG checklist file paths as arguments, the executable will merge (copy) the following from source to target:
    /// <list type="bullet">
    /// <item>source.vulnerability.status</item>
    /// <item>source.vulnerability.comments</item>
    /// <item>soure.vulnerability.finding_details</item>
    /// </list>    
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<MergeOptions>(new List<string>(args))
                .MapResult((MergeOptions opts) => RunMergeAndReturnExitCode(opts), errs => 1);
        }

        private static int RunMergeAndReturnExitCode(MergeOptions opts)
        {
            var cmd = new MergeCommand(opts);
            int result = cmd.Run();
            return result;
        }
    }
}
