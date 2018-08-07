using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Mobicon.Models;
using SharpYaml.Serialization;

namespace Mobicon.Services
{
    public class ImportManager
    {
        public ConfigEntry[] ImportYaml(string data)
        {
            var list = new List<ConfigEntry>();

            var formatter = new Serializer(new SerializerSettings { EmitAlias = false });

            var map = formatter.Deserialize<Dictionary<object, object>>(data);

            Fill("", map, list);

            return list.ToArray();
        }

        private void Fill(string currentPrefix, Dictionary<object, object> map, List<ConfigEntry> list)
        {
            foreach (var kv in map)
            {
                if (kv.Value is Dictionary<object, object> asMap)
                {
                    Fill(Join(":", currentPrefix, kv.Key.ToString()), asMap, list);
                }
                else
                {
                    string keyName = kv.Key.ToString();
                    string[] parts = Regex.Split(keyName, @"(?=[^\d])-(?=[^\d)])").Where(s => !string.IsNullOrEmpty(s)).ToArray();

                    (var simple, var version, var segment) = ExtractPrefixes(parts.Take(parts.Length - 1));

                    list.Add(new ConfigEntry()
                    {
                        Key = Join(":", currentPrefix, parts.Last()),
                        Type = FieldType.String,
                        Value = kv.Value.ToString(),
                        Version = 1,
                        VersionCreateTime = DateTime.Now,
                        VersionPrefix = version,
                        SegmentPrefix = segment,
                        SimplePrefixes = simple.Select(s => new EntryConfigSimplePrefix()
                        {
                            SimplePrefix = s
                        }).ToList()
                    });
                }
            }
        }

        private (SimplePrefix[], VersionPrefix, SegmentPrefix) ExtractPrefixes(IEnumerable<string> parts)
        {
            VersionPrefix version = null;

            var versionPrefixString = parts.SingleOrDefault(s => s.StartsWith("(") && s.EndsWith(")"));
            if (versionPrefixString != null)
            {
                var versionParts = versionPrefixString.Split("-");
                version = new VersionPrefix()
                {
                    From = versionParts.First(),
                    To = versionParts.Last()
                };
            }

            SegmentPrefix segment = null;

            var segmentPrefixString = parts.SingleOrDefault(s => s.StartsWith("<") && s.EndsWith(">"));
            if (segmentPrefixString != null)
            {
                var segmentParts = segmentPrefixString.Split("-");
                segment = new SegmentPrefix()
                {
                    From = int.Parse(segmentParts[0]),
                    To = int.Parse(segmentParts[1])
                };
            }

            SimplePrefix[] simple = parts.Where(p => p != versionPrefixString && p != segmentPrefixString)
                .Select(p => new SimplePrefix()
                {
                    Name = p
                }).ToArray();

            return (simple, version, segment);
        }

        public string Join(string separator, string first, string second)
        {
            if (string.IsNullOrEmpty(first))
                return second;
            else if (string.IsNullOrEmpty(second))
                return first;
            else return first + separator + second;
        }
    }
}
