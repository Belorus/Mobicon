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
                .Include(e => e.SegmentPrefix)
                .Include(e => e.VersionPrefix)
                .Where(e => e.ConfigId == id)
                .GroupBy(e => e.Id)
                .Select(g => g.OrderByDescending(e => e.Version).First())
                .OrderBy(e => e.Key)
                .ToArray();

            FieldTypes = Enum.GetValues(typeof(FieldType)).Cast<FieldType>().Where(f => f != FieldType.Unknown).ToArray();

            return Page();
        }

        public IActionResult OnPostDelete(int id, int entryId)
        {
            var entry = _dataContext.Entries.Find(entryId);
            _dataContext.Entries.Remove(entry);
            _dataContext.SaveChanges();

            return RedirectToPage(new { id = id });
        }

        public IActionResult OnPost(int id, int entryId, string key, string value, string description, string jira, FieldType type)
        {
            var newEntry = new ConfigEntry
            {
                Key = key,
                Value = value,
                Description = description,
                Jira = jira,
                ConfigId = id,
                Type = type,
                Version = 1,
                VersionCreateTime = DateTime.Now,
                VersionCreatedBy = "grigoryp"
            };

            if (entryId > 0)
            {
                var entry = _dataContext.Entries.First(e => e.Id == entryId);

                newEntry.Key = entry.Key;
                newEntry.Version = entry.Version + 1;
                newEntry.SegmentPrefix = entry.SegmentPrefix;
                newEntry.VersionPrefix = entry.VersionPrefix;
            }

            _dataContext.Entries.Add(newEntry);
            _dataContext.SaveChanges();

            return RedirectToPage(new {id = id});
        }
    }
}