using StigChecklistUtil.Helpers;
using StigChecklistUtil.Options;
using System;

namespace StigChecklistUtil.Commands
{
    public class MergeCommand
    {
        private readonly MergeOptions opts;

        public MergeCommand(MergeOptions opts)
        {
            this.opts = opts ?? throw new ArgumentNullException(nameof(opts), "MergeOptions object is required.");
        }
        public int Run()
        {
            int returnValue = 0;

            var sourceFile = opts.SourceFilePath;
            var masterFile = opts.MasterFilePath;
            var overrideTarget = opts.Override;
            var controlFilter = opts.Filter;

            var stigChecklistHelper = new StigChecklistHelper(sourceFile, masterFile);

            stigChecklistHelper.Merge(overrideTarget, controlFilter);

            return returnValue;
        }
    }
}
