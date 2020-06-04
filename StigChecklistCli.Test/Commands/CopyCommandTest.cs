using FluentAssertions;
using StigChecklistCli.Commands;
using StigChecklistCli.Helpers;
using StigChecklistCli.Options;
using System.IO;
using System.Linq;
using Xunit;

namespace StigChecklistCli.Test.Commands
{
    public class CopyCommandTest
    {
        private const string SourceCheckListFile = "ck-3-CopyCommandTest.ckl";
        private const string TargetCheckListFile = "ck-0-CopyCommandTest.ckl";        

        [Fact]
        public void It_can_copy()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var copyOptions = new CopyOptions();
            copyOptions.SourceFilePath = $"{currentDir}/{SourceCheckListFile}";
            copyOptions.TargetFilePath = $"{currentDir}/{TargetCheckListFile}";
            var copyCommand = new CopyCommand(copyOptions);

            copyCommand.Run();

            var expectedFile = $"{currentDir}/ck-0-CopyCommandTest.copied.ckl";

            File.Exists(expectedFile).Should().BeTrue();

            var stigs = StigChecklistHelper.GetChecklist(expectedFile).STIGS[0];
            var vulnerabilities = stigs.VULN.ToList();

            var openCount = vulnerabilities.Where(v => v.STATUS == "Open").Count();
            var notApplicableCount = vulnerabilities.Where(v => v.STATUS == "Not_Applicable").Count();
            var notAFindingCount = vulnerabilities.Where(v => v.STATUS == "NotAFinding").Count();
            var notReviewedCount = vulnerabilities.Where(v => v.STATUS == "Not_Reviewed").Count();

            openCount.Should().Be(1);
            notApplicableCount.Should().Be(1);
            notAFindingCount.Should().Be(1);
            notReviewedCount.Should().BeGreaterThan(1);

            File.Delete(expectedFile);
            File.Exists(expectedFile).Should().BeFalse();
        }

        [Fact]
        public void It_can_not_copy_due_to_filter()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var copyOptions = new CopyOptions();
            copyOptions.SourceFilePath = $"{currentDir}/{SourceCheckListFile}";
            copyOptions.TargetFilePath = $"{currentDir}/{TargetCheckListFile}";
            copyOptions.Filter = "SI";

            var copyCommand = new CopyCommand(copyOptions);

            copyCommand.Run();

            var expectedFile = $"{currentDir}/ck-0-CopyCommandTest.copied.ckl";

            File.Exists(expectedFile).Should().BeTrue();

            var stigs = StigChecklistHelper.GetChecklist(expectedFile).STIGS[0];
            var vulnerabilities = stigs.VULN.ToList();

            var openCount = vulnerabilities.Where(v => v.STATUS == "Open").Count();
            var notApplicableCount = vulnerabilities.Where(v => v.STATUS == "Not_Applicable").Count();
            var notAFindingCount = vulnerabilities.Where(v => v.STATUS == "NotAFinding").Count();
            var notReviewedCount = vulnerabilities.Where(v => v.STATUS == "Not_Reviewed").Count();

            openCount.Should().Be(0);
            notApplicableCount.Should().Be(0);
            notAFindingCount.Should().Be(0);
            notReviewedCount.Should().BeGreaterThan(1);

            File.Delete(expectedFile);
            File.Exists(expectedFile).Should().BeFalse();
        }
    }
}
