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
    public class SnapshotsModel : PageModel
    {
        private readonly DataContext _dataContext;

        public Snapshot[] Snapshots { get; set; }

        public Snapshot LastPublished { get; set; }

        public Segment[] Segments { get; private set; }

        public SnapshotsModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void OnGet()
        {
            Snapshots = _dataContext.Snapshots.Include(s => s.Entries).Include(s => s.Approves).ToArray();
            Segments = _dataContext.Segments.Include(s => s.Configs).ToArray();
            LastPublished = Snapshots.Where(s => s.Status == SnapshotStatus.Published)
                .OrderByDescending(x => x.PublishedAt.Value)
                .FirstOrDefault();
        }

        public IActionResult OnPost(string name, int[] configId)
        {
            var arrayOfArrayOfEntries = _dataContext.Configs.Where(c => configId.Contains(c.Id))
                .Include(c => c.Entries)
                .ThenInclude(c => c.SimplePrefixes)
                .Select(c => 
                    c.Entries
                        .GroupBy(x => x.EntryId)
                        .Select(g => g.OrderByDescending(x => x.Version)
                                      .First())
                        .Where(e => e.IsDeleted == false))
                .ToArray();

            var mergedEntries = ConfigMerger.MergeEntries(arrayOfArrayOfEntries)
                .ToList();

            var configIds = _dataContext.Configs.Where(c => configId.Contains(c.Id))
                .Select(c => c.Id)
                .ToArray();

            var snapshot = new Snapshot
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedBy = User.Identity.Name,
                Name = name,
                CreatedFrom = string.Join(",", configIds)
            };

            snapshot.Entries = mergedEntries.Select(e => new SnapshotToEntry()
            {
                EntryId = e.Id,
                Snapshot = snapshot
            }).ToList();

            _dataContext.Snapshots.Add(snapshot);
            _dataContext.SaveChanges();

            return RedirectToPage();
        }
    }
}