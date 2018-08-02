using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mobicon.Models;

namespace Mobicon.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly DataContext _dataContext;

        public ConfigEntry[] Entries { get; set; }
        public FieldType[] FieldTypes { get; set; }

        public ConfigModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult OnGet(int id)
        {
            Entries = _dataContext.Entries
                .Include(e => e.SimplePrefixes)
                .ThenInclude(e => e.SimplePrefix)
                .Include(e => e.SegmentPrefixes)
                .Include(e => e.VersionPrefixes)
                .Where(e => e.ConfigId == id)
                .GroupBy(e => e.Id)
                .Select(g => g.OrderByDescending(e => e.Version).First())
                .OrderBy(e => e.Key)
                .ToArray();

            FieldTypes = Enum.GetValues(typeof(FieldType)).Cast<FieldType>().Where(f => f != FieldType.Unknown).ToArray();

            return Page();
        }

        public IActionResult OnPost(int id, string key, string value, string description, string jira, FieldType type)
        {
            _dataContext.Entries.Add(new ConfigEntry()
            {
                Key = key,
                Value = value,
                Description = description,
                Jira = jira,
                ConfigId = id,
                Type = type,
                Version = 1
            });

            _dataContext.SaveChanges();

            return OnGet(id);
        }
    }
}