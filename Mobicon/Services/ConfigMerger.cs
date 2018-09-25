using System.Collections.Generic;
using Mobicon.Models;

namespace Mobicon.Services
{
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