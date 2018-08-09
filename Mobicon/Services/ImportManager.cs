using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Mobicon.Models;
using Newtonsoft.Json;
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
                    string keyName = kv.Key.ToString();
                    string[] parts = Regex.Split(keyName, @"(?=[^\d])-(?=[^\d)])").Where(s => !string.IsNullOrEmpty(s)).ToArray();

                    (var simple, var version, var segment) = ExtractPrefixes(parts.Take(parts.Length - 1));

                    if (simple.Any() || version != null || segment != null)
                    {
                        Debug.Assert(false);
                    }

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
                        Type = MapType(kv.Value),
                        Value = JsonConvert.SerializeObject(kv.Value),
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

        private FieldType MapType(object value)
        {
            if (value is long || long.TryParse(value.ToString(), out _))
                return FieldType.Integer;

            if (value is bool || bool.TryParse(value.ToString(), out _))
                return FieldType.Bool;

            if (value is double || double.TryParse(value.ToString(), out _))
                return FieldType.Float;

            if (value is List<object> list)
            {
                if (list.Count > 0)
                {
                    var itemValue = list[0];
                    if (itemValue is long || long.TryParse(itemValue.ToString(), out _))
                        return FieldType.ListOfInteger;

                    if (itemValue is bool || bool.TryParse(itemValue.ToString(), out _))
                        return FieldType.ListOfBool;

                    if (itemValue is double || double.TryParse(itemValue.ToString(), out _))
                        return FieldType.ListOfFloat;
                }

                return FieldType.ListOfString;
            }

            return FieldType.String;
        }

        private (SimplePrefix[], VersionPrefix, SegmentPrefix) ExtractPrefixes(IEnumerable<string> parts)
        {
            VersionPrefix version = null;

            var versionPrefixString = parts.SingleOrDefault(s => s.StartsWith("(") && s.EndsWith(")"));
            if (versionPrefixString != null)
            {
                var versionParts = versionPrefixString.Substring(1, versionPrefixString.Length - 2).Split("-");
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
                var segmentParts = segmentPrefixString.Substring(1, segmentPrefixString.Length - 2).Split("-");
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
