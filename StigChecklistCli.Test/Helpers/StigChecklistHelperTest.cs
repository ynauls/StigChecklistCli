using FluentAssertions;
using StigChecklistCli.Helpers;
using System.IO;
using Xunit;

namespace StigChecklistCli.Test.Helpers
{
    public class StigChecklistHelperTest
    {
        private const string CheckListFileName = "ck-StigCheckListHelperTest.ckl";

        [Fact]
        public void It_can_open_v209_GetCcilist()
        {
            var cciList = StigChecklistHelper.GetCcilist();

            cciList.Should().NotBeNull();
            cciList.cci_items.Length.Should().BeGreaterThan(1);
        }

        [Fact]
        public void It_can_filter_v209_GetCciIds_by_SI()
        {            
            var filtered = StigChecklistHelper.GetCciIds("SI");

            filtered.Should().NotBeNull();
            filtered.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public void It_can_not_filter_v209_GetCciIds_by_invalid_filter()
        {
            var filtered = StigChecklistHelper.GetCciIds("BAD");

            filtered.Should().NotBeNull();
            filtered.Count.Should().Be(0);
        }

        [Fact]
        public void It_can_v210_GetChecklist()
        {
            var checkList = StigChecklistHelper.GetChecklist(CheckListFileName);

            checkList.Should().NotBeNull();
            checkList.STIGS.Length.Should().Be(1);
            checkList.STIGS[0].STIG_INFO.Length.Should().BeGreaterThan(1);
            checkList.STIGS[0].VULN.Length.Should().BeGreaterThan(1);
        }

        [Fact]
        public void It_can_v210_GetStigLookup()
        {
            var checkList = StigChecklistHelper.GetChecklist(CheckListFileName);
            var stigLookup = StigChecklistHelper.GetStigLookup(checkList);

            stigLookup.Should().NotBeNull();
            stigLookup.Count.Should().Be(1);
            stigLookup.ContainsKey("Application_Security_Development_STIG").Should().BeTrue();
        }

        [Fact]
        public void It_can_v210_GetVulnerabilityLookup()
        {
            var checkList = StigChecklistHelper.GetChecklist(CheckListFileName);
            var vulnerabilityLookup = StigChecklistHelper.GetVulnerabilityLookup(checkList.STIGS[0].VULN);

            vulnerabilityLookup.Should().NotBeNull();
            vulnerabilityLookup.Count.Should().BeGreaterThan(1);            
        }

        [Fact]
        public void It_can_SaveChecklist()
        {            
            var checkListFile = $"{Directory.GetCurrentDirectory()}/{CheckListFileName}";
            var checkList = StigChecklistHelper.GetChecklist(CheckListFileName);
            StigChecklistHelper.SaveChecklist(checkList, checkListFile, "unittest");

            var expectedFile = $"{Directory.GetCurrentDirectory()}/ck-StigCheckListHelperTest.unittest.ckl";

            File.Exists(expectedFile).Should().BeTrue();
            File.Delete(expectedFile);
            File.Exists(expectedFile).Should().BeFalse();
        }
    }
}
