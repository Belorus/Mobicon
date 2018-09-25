using System.Collections.Generic;
using System.Linq;
using Mobicon.Models;

namespace Mobicon.Services
{
    public class ConfigEntryComparer : IEqualityComparer<ConfigEntry>
    {
        public bool Equals(ConfigEntry x, ConfigEntry y)
        {
            return x.Key == y.Key &&
                   x.SegmentPrefixFrom == y.SegmentPrefixFrom &&
                   x.SegmentPrefixTo == y.SegmentPrefixTo &&
                   x.VersionPrefixFrom == y.VersionPrefixFrom &&
                   x.VersionPrefixTo == y.VersionPrefixTo &&
                   (x.SimplePrefixes == y.SimplePrefixes ||
                Enumerable.SequenceEqual(
                    x.SimplePrefixes.Select(sp => sp.SimplePrefixId),
                    y.SimplePrefixes.Select(sp => sp.SimplePrefixId)));
        }

        public int GetHashCode(ConfigEntry obj)
        {
            return obj.Key.GetHashCode();
        }
    }

    public static class ConfigMerger
    {
        public static HashSet<ConfigEntry> MergeEntries(IEnumerable<IEnumerable<ConfigEntry>> entries)
        {
            var hash = new HashSet<ConfigEntry>(new ConfigEntryComparer());

            foreach (var config in entries.Reverse())
            {
                foreach (var entry in config)
                {
                    hash.Add(entry);
                }
            }

            return hash;
        }
    }
}