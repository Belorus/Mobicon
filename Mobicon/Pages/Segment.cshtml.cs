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
    public class SegmentModel : PageModel
    {
        private readonly DataContext _dataContext;
        private readonly ImportManager _importManager;

        public string Name { get; set; }
        public Config[] Configs { get; private set; }

        public SegmentModel(
            DataContext dataContext,
            ImportManager importManager)
        {
            _dataContext = dataContext;
            _importManager = importManager;
        }

        public void OnGet(int id)
        {
            Configs = _dataContext.Configs.Include(x => x.Entries).Where(c => c.SegmentId == id).ToArray();
            Name = _dataContext.Segments.Find(id).Name;
        }

        public IActionResult OnPostImport(int id, string name, string data)
        {
            var entries = _importManager.ImportYaml(data, User.Identity.Name);

            _dataContext.Configs.Add(new Config()
            {
                Name = name,
                SegmentId = id,
                Entries = entries.ToList(),
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedAt = DateTime.Now,
                UpdatedBy = User.Identity.Name
            });

            _dataContext.SaveChanges();

            return RedirectToPage(new {id = id});
        }

        public IActionResult OnPostDelete(int id)
        {
            if (User.IsInRole(UserRole.Admin.ToString()))
            {
                _dataContext.Segments.Remove(_dataContext.Segments.Find(id));
                _dataContext.SaveChanges();
            }

            return RedirectToPage("Segments");
        }

        public IActionResult OnPost(int id, string configName)
        {
            _dataContext.Configs.Add(new Config()
            {
                Name = configName,
                SegmentId = id,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedAt = DateTime.Now,
                UpdatedBy = User.Identity.Name
            });

            _dataContext.SaveChanges();

            return RedirectToPage(new {id = id});
        }
    }
}