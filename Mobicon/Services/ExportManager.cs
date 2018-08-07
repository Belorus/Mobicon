using System.IO;
using Mobicon.Models;

namespace Mobicon.Services
{
    public class ExportManager
    {
        public Stream ExportToJson(ConfigEntry[] entries)
        {
            return Stream.Null;
        }

        public Stream ExportToYaml(ConfigEntry[] entries)
        {
            return Stream.Null;
        }
    }
}
