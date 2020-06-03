using System;
using System.Collections.Generic;

namespace StigChecklistCli.Models
{
    public class StigKey : IComparer<StigKey>, IEquatable<StigKey>
    {
        public string VulnerabilityNumber { get; set; }

        #region Metadata

        public IList<string> CiiIds { get; set; }

        #endregion        

        public int Compare(StigKey x, StigKey y)
        {
            return x.VulnerabilityNumber.CompareTo(y.VulnerabilityNumber);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StigKey);
        }

        public bool Equals(StigKey other)
        {
            return other != null && VulnerabilityNumber == other.VulnerabilityNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(VulnerabilityNumber);
        }
    }
}
