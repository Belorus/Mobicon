﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mobicon.Models;

namespace Mobicon.Pages
{
    [Authorize]
    public class ConfigModel : PageModel
    {
        private readonly DataContext _dataContext;

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public Snapshot LastPublished { get; set; }
        public ConfigEntry[] Entries { get; set; }
        public FieldType[] FieldTypes { get; set; }
        public SimplePrefix[] SimplePrefixes { get; set; }
        public Snapshot[] UnpublishedSnapshots { get; set; }

        public ConfigModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult OnGet(int id)
        {
            var config = _dataContext.Configs.Find(id);
            if (config.IsDeleted)
                return RedirectToPage("Configs");

            Entries = _dataContext.Entries
                .Include(e => e.SimplePrefixes)
                .ThenInclude(e => e.SimplePrefix)
                .Where(e => e.ConfigId == id)
                .GroupBy(x => x.EntryId)
                .Select(g => g.OrderByDescending(x => x.Version).First())
                .Where(e => e.IsDeleted == false)
                .OrderBy(e => e.Key)
                .ToArray();

            Id = id;
            Name = config.Name;
            CreatedBy = config.CreatedBy;
            FieldTypes = Enum.GetValues(typeof(FieldType)).Cast<FieldType>().Where(f => f != FieldType.Unknown).ToArray();

            UnpublishedSnapshots = _dataContext.Snapshots
                .Where(s => s.Status == SnapshotStatus.WaitingForApprove)
                .ToArray();
            SimplePrefixes = _dataContext.SimplePrefixes.ToArray();
            LastPublished = _dataContext.Snapshots
                .Include(s => s.Entries)
                .ThenInclude(e => e.Entry)
                .Where(s => s.Status == SnapshotStatus.Published)
                .OrderByDescending(s => s.PublishedAt.Value)
                .FirstOrDefault();

            return Page();
        }

        public IActionResult OnPostDeleteConfig(int id)
        {
            if (User.IsInRole(UserRole.Admin.ToString()))
            {
                var config = _dataContext.Configs.Find(id);
                if (config != null)
                {
                    config.IsDeleted = true;
                    _dataContext.SaveChanges();

                    return RedirectToPage("Configs");
                }
            }

            return RedirectToPage(new { id = id });
        }

        public IActionResult OnPostDelete(int id, int entryId)
        {
            var entry = _dataContext.Entries.Include(x => x.SimplePrefixes).First(e => e.Id == entryId);

            var newEntry = new ConfigEntry
            {
                Key = entry.Key,
                Value = entry.Value,
                ConfigId = id,
                VersionCreateTime = DateTime.Now,
                VersionCreatedBy = User.Identity.Name,
                EntryId = entry.EntryId,
                Version = entry.Version + 1,
                SimplePrefixes = entry.SimplePrefixes.Select(p => new EntryConfigSimplePrefix()
                {
                    SimplePrefixId = p.SimplePrefixId
                }).ToList(),
                SegmentPrefixFrom = entry.SegmentPrefixFrom,
                SegmentPrefixTo = entry.SegmentPrefixTo,
                VersionPrefixFrom = entry.VersionPrefixFrom,
                VersionPrefixTo = entry.VersionPrefixTo,
                IsDeleted = true,
            };

            var config = _dataContext.Configs.Find(id);
            config.UpdatedAt = DateTime.Now;
            config.UpdatedBy = User.Identity.Name;

            _dataContext.Entries.Add(newEntry);
            _dataContext.SaveChanges();

            

            return RedirectToPage(new { id = id });
        }

        public IActionResult OnPost(int id, int entryId, string key, string value, string description, string jira, FieldType type, string versionFrom, string versionTo, int? segmentFrom, int? segmentTo, int[] simplePrefixes)
        {
            if (type == FieldType.String && !(value.StartsWith("\"") && value.EndsWith("\"")))
                value = "\"" + value + "\"";

            var newEntry = new ConfigEntry
            {
                Key = key,
                Value = value,
                Description = description,
                Jira = jira,
                ConfigId = id,
                Type = type,
                VersionCreateTime = DateTime.Now,
                VersionCreatedBy = User.Identity.Name,
            };

            if (entryId > 0)
            {
                // Edit
                var entry = _dataContext.Entries.Include(x => x.SimplePrefixes).First(e => e.Id == entryId);

                newEntry.Key = entry.Key;
                newEntry.EntryId = entry.EntryId;
                newEntry.Version = entry.Version + 1;
                newEntry.SimplePrefixes = entry.SimplePrefixes.Select(p => new EntryConfigSimplePrefix()
                     {
                         SimplePrefixId = p.SimplePrefixId
                     }).ToList();
                newEntry.SegmentPrefixFrom = entry.SegmentPrefixFrom;
                newEntry.SegmentPrefixTo = entry.SegmentPrefixTo;
                newEntry.VersionPrefixFrom = entry.VersionPrefixFrom;
                newEntry.VersionPrefixTo = entry.VersionPrefixTo;
            }
            else
            {
                // Add 

                newEntry.VersionPrefixFrom = versionFrom;
                newEntry.VersionPrefixTo = versionTo;
                newEntry.SegmentPrefixFrom = segmentFrom;
                newEntry.SegmentPrefixTo = segmentTo;

                newEntry.EntryId = Guid.NewGuid().ToString("N");
                newEntry.Version = 1;
                newEntry.SimplePrefixes = simplePrefixes.Select(sid => new EntryConfigSimplePrefix()
                {
                    SimplePrefixId = sid
                }).ToList();
            }

            var config = _dataContext.Configs.Find(id);
            config.UpdatedAt = DateTime.Now;
            config.UpdatedBy = User.Identity.Name;

            _dataContext.Entries.Add(newEntry);
            _dataContext.SaveChanges();

            return RedirectToPage(new {id = id});
        }

        public IActionResult OnPostPromoteKeys(int id, string snapshotName, int[] entryIds)
        {
            var lastPublished = _dataContext.Snapshots
                .Include(s => s.Entries)
                .ThenInclude(e => e.Entry)
                .Where(s => s.Status == SnapshotStatus.Published)
                .OrderByDescending(x => x.PublishedAt.Value)
                .First();

            var newEntries = _dataContext.Entries.Where(e => entryIds.Contains(e.Id))
                .ToArray();

            var snapshot = new Snapshot()
            {
                Name = snapshotName,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedAt = DateTime.Now,
                UpdatedBy = User.Identity.Name
            };

            snapshot.Entries = lastPublished.Entries.Select(se => new SnapshotToEntry()
            {
                EntryId = se.EntryId,
                Snapshot = snapshot
            }).ToList();
            

            foreach (var newEntry in newEntries)
            {
                var existingEntry = lastPublished.Entries.FirstOrDefault(x => x.Entry.EntryId == newEntry.EntryId);
                if (existingEntry != null)
                {
                    var toRemove = snapshot.Entries.First(x => x.EntryId == existingEntry.EntryId);
                    snapshot.Entries.Remove(toRemove);
                }

                snapshot.Entries.Add(new SnapshotToEntry
                {
                    SnapshotId = id,
                    EntryId = newEntry.Id
                });
            }

            _dataContext.Snapshots.Add(snapshot);
            _dataContext.SaveChanges();

            return RedirectToPage("Snapshot", new {id = snapshot.Id});
        }
    }
}