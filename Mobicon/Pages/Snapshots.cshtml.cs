using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mobicon.Models;

namespace Mobicon.Pages
{
    [Authorize]
    public class SnapshotsModel : PageModel
    {
        private readonly DataContext _dataContext;

        public Snapshot[] Snapshots { get; set; }

        public Segment[] Segments { get; private set; }

        public SnapshotsModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void OnGet()
        {
            Snapshots = _dataContext.Snapshots.Include(s => s.Entries).ToArray();
            Segments = _dataContext.Segments.Include(s => s.Configs).ToArray();
        }

        public IActionResult OnPost(string name, int[] configId)
        {
            var entries = _dataContext.Configs.Where(c => configId.Contains(c.Id))
                .Include(c => c.Entries)
                .SelectMany(c => c.Entries)
                .GroupBy(x => x.EntryId)
                .Select(g => g.OrderByDescending(x => x.Version).First())
                .Where(e => e.IsDeleted == false)
                .ToList();

            var snapshot = new Snapshot
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedBy = User.Identity.Name,
                Name = name
            };

            snapshot.Entries = entries.Select(e => new SnapshotToEntry()
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