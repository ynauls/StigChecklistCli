using CommandLine;

namespace StigChecklistCli.Options
{
    [Verb(name: "copy", HelpText = "copies STIG Viewer Checklist content of Checklist A to Checklist B, and saves CheckList B with '*.copied.ckl' file for all STIGs NOT Reviewed.")]
    public class CopyOptions
    {
        [Option('s', "source", Required = true, HelpText = "Location of Source file.")]
        public string SourceFilePath { get; set; }

        [Option('t', "target", Required = true, HelpText = "Location of Target file.")]
        public string TargetFilePath { get; set; }

        [Option('o', "override", Required = false, Default = false, HelpText = "Overrides content regardless the status of the STIG.")]
        public bool Override { get; set; }

        [Option('f', "filter", Required = false, HelpText = "Copies only STIGs with matching Control ID, such as AU, SI, IA, etc. (supports single control only).")]
        public string Filter { get; set; }
    }
}
