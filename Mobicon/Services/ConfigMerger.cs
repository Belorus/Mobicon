using System.Collections.Generic;
using System.Linq;
using Mobicon.Models;

namespace Mobicon.Pages
{
    public class EntryEqualityComparer : IEqualityComparer<ConfigEntry>{
        public bool Equals(ConfigEntry x, ConfigEntry y)
        {
            if (!string.Equals(x.Key, y.Key))
                return false;

                if (x.VersionPrefixFrom != y.VersionPrefixFrom ||
                    x.VersionPrefixTo != y.VersionPrefixTo)
                    return false;

                if (x.SegmentPrefixFrom != y.SegmentPrefixFrom ||
                    x.SegmentPrefixTo != y.SegmentPrefixTo)
                    return false;

            if (!Enumerable.SequenceEqual(
                x.SimplePrefixes.Select(p => p.SimplePrefix.Id),
                y.SimplePrefixes.Select(p => p.SimplePrefix.Id)))
                return false;

            return true;
        }

        public int GetHashCode(ConfigEntry obj)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ConfigMerger
    {
        public IReadOnlyCollection<ConfigEntry> MergeEntries(IReadOnlyList<Config> configs)
        {
            var hash = new HashSet<ConfigEntry>();

            foreach (var config in configs)
            {
                foreach (var entry in config.Entries)
                {
                    hash.Add(entry);
                }
            }

            return hash;
        }
    }
}