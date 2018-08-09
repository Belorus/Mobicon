using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mobicon.Models;
using Mobicon.Services;

namespace Mobicon.Pages
{
    [Authorize]
    public class SnapshotModel : PageModel
    {
        private readonly DataContext _dataContext;
        private readonly ExportManager _exportManager;

        public Snapshot[] Snapshots{ get; set; }

        public Snapshot Snapshot{ get; set; }

        public SnapshotModel(
            DataContext dataContext,
            ExportManager exportManager)
        {
            _dataContext = dataContext;
            _exportManager = exportManager;
        }

        public IActionResult OnPostExport(int id, ExportFormat exportFormat)
        {
            var snapshot = LoadSnapshot(5);

            switch (exportFormat)
            {
                case ExportFormat.Json:
                    return File(_exportManager.ExportToJson(snapshot.Entries.Select(e => e.Entry).ToArray()), "application/json", snapshot.Name + ".json");
                case ExportFormat.Yaml:
                    return File(_exportManager.ExportToYaml(snapshot.Entries.Select(e => e.Entry).ToArray()), "application/yaml", snapshot.Name + ".yml");
                default:
                    throw new NotImplementedException();
            }
        }

        public void OnGet(int id)
        {
            Snapshot = LoadSnapshot(id);

            Snapshots = _dataContext.Snapshots
                .Where(s => s.Id != id)
                .OrderByDescending(s => s.UpdatedAt)
                .ToArray();
        }

        private Snapshot LoadSnapshot(int id)
        {
            return _dataContext.Snapshots
                .Include(e => e.Entries)
                .ThenInclude(e => e.Entry)
                .ThenInclude(e => e.SimplePrefixes)
                .ThenInclude(e => e.SimplePrefix)
                .Include(e => e.Entries)
                .ThenInclude(e => e.Entry)
                .ThenInclude(e => e.SegmentPrefix)
                .Include(e => e.Entries)
                .ThenInclude(e => e.Entry)
                .ThenInclude(e => e.VersionPrefix)
                .First(x => x.Id == id);
        }
    }
}