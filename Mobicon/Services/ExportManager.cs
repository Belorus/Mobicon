using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mobicon.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpYaml.Serialization;

namespace Mobicon.Services
{
    public class ExportManager
    {
        public Stream ExportToJson(IReadOnlyCollection<ConfigEntry> entries)
        {
            var map = new Dictionary<object, object>();
            ConvertToMap(entries, map, 0);

            var ms = new MemoryStream();

            var formatter = new Serializer(new SerializerSettings
            {
                SortKeyForMapping = false,
                EmitTags = false,
                PreferredIndent = 4,
                LimitPrimitiveFlowSequence = 25,
                EmitAlias = false,
                EmitJsonComptible = true
            });
            formatter.Serialize(ms, map);
            ms.Position = 0;

            return ms;
        }

        public Stream ExportToYaml(IReadOnlyCollection<ConfigEntry> entries)
        {
            var map = new Dictionary<object, object>();
            ConvertToMap(entries, map, 0);

            var ms = new MemoryStream();

            var formatter = new Serializer(new SerializerSettings
            {
                SortKeyForMapping = false,
                EmitTags = false,
                PreferredIndent = 4,
                LimitPrimitiveFlowSequence = 25,
                EmitAlias = false,
            });
            formatter.Serialize(ms, map);
            ms.Position = 0;

            return ms;
        }

        private void ConvertToMap(IReadOnlyCollection<ConfigEntry> entries, Dictionary<object, object> map, int depth)
        {
            foreach (IGrouping<string, ConfigEntry> g in entries.GroupBy(e => e.Key.Split(':')[depth]))
            {
                bool isValue = false;
                foreach (ConfigEntry item in g)
                {

                    if (item.Key.Count(c => c == ':') == depth)
                    {
                        var key = string.Join("-", item.SimplePrefixes.Select(x => x.SimplePrefix.Name));
                        if (item.SegmentPrefix != null)
                            key += "<" + item.SegmentPrefix.From + ".." + item.SegmentPrefix.To + ">";
                        if (item.VersionPrefix != null)
                            key += "(" + item.VersionPrefix.From + "-" + item.VersionPrefix.To + ")";

                        if (key != string.Empty)
                            key = "-" + key + "-";

                        object val;
                        if (
                            item.Type == FieldType.ListOfBool ||
                            item.Type == FieldType.ListOfString ||
                            item.Type == FieldType.ListOfInteger ||
                            item.Type == FieldType.ListOfFloat)
                        {
                            val = JsonConvert.DeserializeObject<IList<object>>(item.Value);
                            if (((IList<object>) val).OfType<JToken>().Any())
                            {
                                val = "UNSUPPORTED";
                            }
                        }
                        else
                        {
                            val = JsonConvert.DeserializeObject(item.Value);
                            if (val is JToken)
                            {
                                val = "UNSUPPORTED";
                            }
                        }
                        
                        map[key + g.Key] = val;

                        isValue = true;
                    }
                }

                if (!isValue)
                {
                    var innerMap = new Dictionary<object, object>();
                    map[g.Key] = innerMap;
                    ConvertToMap(g.ToArray(), innerMap, depth + 1);
                }
            }
        }
    }
}
