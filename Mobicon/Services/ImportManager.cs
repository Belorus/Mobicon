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
        private readonly DataContext _dataContext;

        public ImportManager(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

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
                        Type = MapType(kv.Value),
                        EntryId = Guid.NewGuid().ToString("N"),
                        Value = JsonConvert.SerializeObject(kv.Value),
                        Version = 1,
                        VersionCreateTime = DateTime.Now,
                        VersionPrefixFrom = version.Item1,
                        VersionPrefixTo = version.Item2,
                        SegmentPrefixFrom = segment.Item1,
                        SegmentPrefixTo = segment.Item2,
                        SimplePrefixes = simple.Select(s => new EntryConfigSimplePrefix()
                        {
                            SimplePrefixId = s.Id
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

        private (SimplePrefix[], (string, string), (int?, int?)) ExtractPrefixes(IEnumerable<string> parts)
        {
            (string, string) version = (null, null);

            var versionPrefixString = parts.SingleOrDefault(s => s.StartsWith("(") && s.EndsWith(")"));
            if (versionPrefixString != null)
            {
                var versionParts = versionPrefixString.Substring(1, versionPrefixString.Length - 2).Split("-");
                version = (versionParts.First(), versionParts.Last());
            }

            (int?, int?) segment = (null, null);

            var segmentPrefixString = parts.SingleOrDefault(s => s.StartsWith("<") && s.EndsWith(">"));
            if (segmentPrefixString != null)
            {
                var segmentParts = segmentPrefixString.Substring(1, segmentPrefixString.Length - 2).Split("-");
                segment = (int.Parse(segmentParts[0]), int.Parse(segmentParts[1]));
            }

            SimplePrefix[] simple = parts.Where(p => p != versionPrefixString && p != segmentPrefixString)
                .Select(p => _dataContext.SimplePrefixes.First(prefix => prefix.Name == p)).ToArray();

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
