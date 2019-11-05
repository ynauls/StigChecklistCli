using CommandLine;

namespace StigChecklistUtil.Options
{
    [Verb(name: "merge", HelpText = "merges STIG Viewer Checklist content of Source and Master files into '*.merged.ckl' file for all STIGs NOT Reviewed.")]
    public class MergeOptions
    {
        [Option('s', "source", Required = true, HelpText = "Location of Source file.")]
        public string SourceFilePath { get; set; }

        [Option('m', "master", Required = true, HelpText = "Location of Master file.")]
        public string MasterFilePath { get; set; }

        [Option('o', "override", Required = false, Default = false, HelpText = "Overrides content regardless the status of the STIG.")]
        public bool Override { get; set; }

        [Option('f', "filter", Required = false, HelpText = "Merges only STIGs with matching Control, such as AU, SI, IA, etc. (supports single control only).")]
        public string Filter { get; set; }
    }
}
