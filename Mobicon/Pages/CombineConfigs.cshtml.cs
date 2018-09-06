using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mobicon.Models;
using Mobicon.Services;

namespace Mobicon.Pages
{
    public class CombineConfigsModel : PageModel
    {
        private readonly DataContext _dataContext;

        public CombineConfigsModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult OnGetYaml(string parts)
        {
            var entries = LoadEntries(parts);
            var exportManager = new ExportManager();
            var resultStream = exportManager.ExportToYaml(entries);

            return File(resultStream, "application/yaml");
        }

        public IActionResult OnGetJson(string parts)
        {
            var entries = LoadEntries(parts);
            var exportManager = new ExportManager();
            var resultStream = exportManager.ExportToJson(entries);

            return File(resultStream, "application/json");
        }

        private IReadOnlyCollection<ConfigEntry> LoadEntries(string parts)
        {
            var parsedArguments = parts
                .Split(';', ',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split('=', ':'));

            var list = new List<Config>();

            foreach (var arr in parsedArguments.OrderBy(arr => arr[0]))
            {
                var segment = _dataContext.Segments
                    .Include(x => x.Configs)
                    .ThenInclude(x => x.Entries)
                    .ThenInclude(x => x.VersionPrefix)
                    .Include(x => x.Configs)
                    .ThenInclude(x => x.Entries)
                    .ThenInclude(x => x.SegmentPrefix)
                    .Include(x => x.Configs)
                    .ThenInclude(x => x.Entries)
                    .ThenInclude(x => x.SimplePrefixes)
                    .ThenInclude(x => x.SimplePrefix)
                    .First(x => x.Name == arr[0]);

                var config = segment.Configs.First(x => x.Name == arr[1]);

                list.Add(config);
            }

            var configMerger = new ConfigMerger();
            var entries = configMerger.MergeEntries(list);
            return entries;
        }
    }
}