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

        public int Id { get; set; }
        public int? ComparedWithId { get; set; }
        public Snapshot[] Snapshots{ get; set; }
        public EntryDiff[] Entries { get; set; }

        public SnapshotModel(
            DataContext dataContext,
            ExportManager exportManager)
        {
            _dataContext = dataContext;
            _exportManager = exportManager;
        }

        public IActionResult OnPostExport(int id, ExportFormat exportFormat)
        {
            var snapshot = LoadSnapshot(1);

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

            return Page();
        }

        private EntryDiff[] Compare(Snapshot snapshot, Snapshot snapshotToCompareWith)
        {
            var curr = snapshot.Entries.Select(x => x.Entry).ToArray();
            var old = snapshotToCompareWith.Entries.Select(x => x.Entry).ToArray();

            var added = curr.Except(old, Pages.Compare.By<ConfigEntry, int>(x => x.Id)).Select(x => new EntryDiff(x, Difference.Added));
            var deleted = old.Except(curr, Pages.Compare.By<ConfigEntry, int>(x => x.Id)).Select(x => new EntryDiff(x, Difference.Removed));
            var unchanged = curr.ToHashSet().Intersect(old, Pages.Compare.By<ConfigEntry, int>(x => x.Id)).Select(x => new EntryDiff(x, Difference.None));

            return added.Concat(deleted).Concat(unchanged).ToArray();
        }

        public IActionResult OnGet(int id)
        {
            var snapshot = LoadSnapshot(id);

            Id = id;
            Entries = snapshot.Entries.Select(e => new EntryDiff(e.Entry, Difference.None)).ToArray();
            Snapshots = _dataContext.Snapshots
                .Where(s => s.Id != id)
                .OrderByDescending(s => s.UpdatedAt)
                .ToArray();

            return Page();
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

    public enum Difference
    {
        None,
        Added,
        Removed,
        Changed
    }

    public class EntryDiff
    {
        public EntryDiff(ConfigEntry entry, Difference difference)
        {
            Entry = entry;
            Difference = difference;
        }

        public ConfigEntry Entry;
        public Difference Difference;
    }
}