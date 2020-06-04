using CommandLine;
using StigChecklistCli.Commands;
using StigChecklistCli.Options;
using System.Collections.Generic;

namespace StigChecklistCli
{
    /// <summary>
    /// Given a source and target STIG checklist file paths as arguments, the executable will copy the following from source to target:
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
            return Parser.Default.ParseArguments<CopyOptions>(new List<string>(args))
                .MapResult((CopyOptions opts) => RunCopyAndReturnExitCode(opts), errs => 1);
        }

        private static int RunCopyAndReturnExitCode(CopyOptions opts)
        {
            var cmd = new CopyCommand(opts);
            int result = cmd.Run();
            return result;
        }
    }
}
