﻿using System;
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
        private readonly AppSettings _settings;

        public SnapshotStatus Status { get; set; }
        public int ApprovesToPublish { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ComparedWithId { get; set; }
        public Snapshot[] Snapshots { get; set; }
        public EntrySnapshot[] Entries { get; set; }
        public SnapshotApproval[] Approves { get; set; }

        public SnapshotModel(
            DataContext dataContext,
            ExportManager exportManager,
            AppSettings settings)
        {
            _dataContext = dataContext;
            _exportManager = exportManager;
            _settings = settings;
        }

        public IActionResult OnPostExport(int id, ExportFormat exportFormat)
        {
            var snapshot = LoadSnapshot(id);

            switch (exportFormat)
            {
                case ExportFormat.Json:
                    return File(_exportManager.ExportToJson(snapshot.Entries.Select(e => e.Entry).ToArray()),
                        "application/json", snapshot.Name + ".json");
                case ExportFormat.Yaml:
                    return File(_exportManager.ExportToYaml(snapshot.Entries.Select(e => e.Entry).ToArray()),
                        "application/yaml", snapshot.Name + ".yml");
                default:
                    throw new NotImplementedException();
            }
        }

        public IActionResult OnPostCompared(int id, int? compareWithId)
        {
            if (!compareWithId.HasValue)
                return OnGet(id);

            var snapshot = LoadSnapshot(id);

            var snapshotToCompareWith = LoadSnapshot(compareWithId.Value);

            var diff = Compare(snapshot, snapshotToCompareWith);

            ComparedWithId = compareWithId;
            Id = id;
            Entries = diff;
            Snapshots = _dataContext.Snapshots
                .Where(s => s.Id != id)
                .OrderByDescending(s => s.UpdatedAt)
                .ToArray();
            Approves = _dataContext.SnapshotApprovals
                .Where(a => a.SnapshotId == id)
                .ToArray();
            Status = snapshot.Status;
            Name = snapshot.Name + " compared with " + snapshotToCompareWith.Name;
            ApprovesToPublish = _settings.ApprovalsBeforePublish;

            return Page();
        }

        public IActionResult OnPostUpdateEntry(int id, string entryId, int version)
        {
            var snapshot = _dataContext.Snapshots.Include(x => x.Entries).ThenInclude(x => x.Entry).First(s => s.Id == id);

            if (snapshot.Status != SnapshotStatus.Published)
            {
                var desiredEntryId = _dataContext.Entries.First(e => e.EntryId == entryId && e.Version == version).Id;

                var entry = snapshot.Entries.First(e => e.Entry.EntryId == entryId);

                snapshot.Entries.Remove(entry);
                snapshot.Entries.Add(new SnapshotToEntry()
                {
                    EntryId = desiredEntryId,
                    SnapshotId = id
                });

                _dataContext.SaveChanges();
            }

            return RedirectToPage(new {id = id});
        }

        private EntrySnapshot[] Compare(Snapshot snapshot, Snapshot snapshotToCompareWith)
        {
            var curr = snapshot.Entries.Select(x => x.Entry).ToArray();
            var old = snapshotToCompareWith.Entries.Select(x => x.Entry).ToArray();

            var added = curr.Except(old, Pages.Compare.By<ConfigEntry, int>(x => x.Id))
                .Select(x => new EntrySnapshot(x, Difference.Added, new ConfigEntry[0]));
            var deleted = old.Except(curr, Pages.Compare.By<ConfigEntry, int>(x => x.Id))
                .Select(x => new EntrySnapshot(x, Difference.Removed, new ConfigEntry[0]));
            var unchanged = curr.ToHashSet().Intersect(old, Pages.Compare.By<ConfigEntry, int>(x => x.Id))
                .Select(x => new EntrySnapshot(x, Difference.None, new ConfigEntry[0]));

            return added.Concat(deleted).Concat(unchanged).ToArray();
        }

        public IActionResult OnPostApprove(int id)
        {
            _dataContext.SnapshotApprovals.Add(new SnapshotApproval
            {
                Username = User.Identity.Name,
                ApprovedAt = DateTime.Now,
                SnapshotId = id
            });


            if (_dataContext.SnapshotApprovals.Count(a => a.SnapshotId == id) >= _settings.ApprovalsBeforePublish - 1)
            {
                var snapshot = _dataContext.Snapshots.Find(id);
                snapshot.Status = SnapshotStatus.Published;
                snapshot.PublishedAt = DateTime.Now;
                snapshot.UpdatedAt = DateTime.Now;
                snapshot.UpdatedBy = User.Identity.Name;
            }

            _dataContext.SaveChanges();

            return RedirectToPage(new { id = id });
        }

        public IActionResult OnPostDisapprove(int id)
        {
            if (_dataContext.Snapshots.Find(id).Status == SnapshotStatus.WaitingForApprove)
            {
                var itemToRemove = _dataContext.SnapshotApprovals.First(a => a.Username == User.Identity.Name && a.SnapshotId == id);

                _dataContext.SnapshotApprovals.Remove(itemToRemove);
                _dataContext.SaveChanges();
            }

            return RedirectToPage(new {id = id});
        }

        public IActionResult OnPostDeleteEntry(int id, int entryUniqueId)
        {
            var snapshot = _dataContext.Snapshots.Include(s => s.Entries).First(s => s.Id == id);

            if (snapshot.Status != SnapshotStatus.Published)
            {
                var entryToDelete = snapshot.Entries.First(se => se.EntryId == entryUniqueId);
                snapshot.Entries.Remove(entryToDelete);
                _dataContext.SaveChanges();
            }

            return RedirectToPage(new {id = id});
        }

        public IActionResult OnPostDelete(int id)
        {
            var snapshot = _dataContext.Snapshots.Find(id);

            if (snapshot.Status != SnapshotStatus.Published)
            {
                _dataContext.Snapshots.Remove(snapshot);
                _dataContext.SaveChanges();

                return RedirectToPage("Snapshots");
            }
            else
            {
                return RedirectToPage(new {id = id});
            }
        }

        public IActionResult OnGet(int id)
        {
            var snapshot = LoadSnapshot(id);

            Id = id;
            var allEntryIds = snapshot.Entries.Select(e => e.Entry.EntryId).ToHashSet();
            ILookup<string, ConfigEntry> lookup = _dataContext.Entries.Where(ce => allEntryIds.Contains(ce.EntryId)).ToLookup(e => e.EntryId);
            Entries = snapshot.Entries.Select(e => new EntrySnapshot(e.Entry, Difference.None, lookup[e.Entry.EntryId].Where(ce => ce.Id != e.EntryId).ToArray())).ToArray();
            Snapshots = _dataContext.Snapshots
                .Where(s => s.Id != id)
                .OrderByDescending(s => s.UpdatedAt)
                .ToArray();
            Approves = _dataContext.SnapshotApprovals
                .Where(a => a.SnapshotId == id)
                .ToArray();

            Status = snapshot.Status;
            Name = snapshot.Name;
            ApprovesToPublish = _settings.ApprovalsBeforePublish;

            return Page();
        }

        private Snapshot LoadSnapshot(int id)
        {
            return _dataContext.Snapshots
                .Include(e => e.Entries)
                .ThenInclude(e => e.Entry)
                .ThenInclude(e => e.SimplePrefixes)
                .ThenInclude(e => e.SimplePrefix)
                .First(x => x.Id == id);
        }
    }

    public enum Difference
    {
        None,
        Added,
        Removed,
        Changed
    }

    public class EntrySnapshot
    {
        public EntrySnapshot(ConfigEntry entry, Difference difference, ConfigEntry[] entryVersions)
        {
            Entry = entry;
            Difference = difference;
            EntryVersions = entryVersions;
        }

        public ConfigEntry Entry;
        public ConfigEntry[] EntryVersions;
        public Difference Difference;
    }
}