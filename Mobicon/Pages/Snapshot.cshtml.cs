using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mobicon.Models;

namespace Mobicon.Pages
{
    [Authorize]
    public class SnapshotModel : PageModel
    {
        private readonly DataContext _dataContext;

        public ConfigEntry[] Entries { get; set; }

        public Snapshot[] Snapshots{ get; set; }

        public SnapshotModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void OnPostExport(ExportFormat format)
        {
            
        }

        public void OnGet(int id)
        {
            Entries = _dataContext.Snapshots
                .Include(e => e.Entries)
                .ThenInclude(e => e.Entry)
                .ThenInclude(e => e.SimplePrefixes)
                .Include(e => e.Entries)
                .ThenInclude(e => e.Entry)
                .ThenInclude(e => e.SegmentPrefix)
                .Include(e => e.Entries)
                .ThenInclude(e => e.Entry)
                .ThenInclude(e => e.VersionPrefix)
                .First(x => x.Id == id)
                .Entries
                .Select(e => e.Entry)
                .ToArray();

            Snapshots = _dataContext.Snapshots
                .Where(s => s.Id != id)
                .OrderByDescending(s => s.UpdatedAt)
                .ToArray();
        }
    }
}