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
    public class ConfigsModel : PageModel
    {
        private readonly DataContext _dataContext;
        private readonly ImportManager _importManager;

        public Segment[] Segments { get; private set; }

        public ConfigsModel(
            DataContext dataContext,
            ImportManager importManager)
        {
            _dataContext = dataContext;
            _importManager = importManager;
        }

        public void OnGet()
        {
            Segments = _dataContext.Segments.Include(s => s.Configs).ToArray();
        }

        public IActionResult OnPost(string segmentName)
        {
            _dataContext.Segments.Add(new Segment()
            {
                Name = segmentName,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name
            });

            _dataContext.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostImport(int id, string name, string data)
        {
            var entries = _importManager.ImportYaml(data, User.Identity.Name);

            var config = new Config()
            {
                Name = name,
                SegmentId = id,
                Entries = entries.ToList(),
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedAt = DateTime.Now,
                UpdatedBy = User.Identity.Name
            };

            _dataContext.Configs.Add(config);

            _dataContext.SaveChanges();

            return RedirectToPage("Config", new { id = config.Id });
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

        public IActionResult OnPostAddConfig(int id, string configName)
        {
            var config = new Config
            {
                Name = configName,
                SegmentId = id,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedAt = DateTime.Now,
                UpdatedBy = User.Identity.Name
            };
            _dataContext.Configs.Add(config);

            _dataContext.SaveChanges();

            return RedirectToPage("Config", new {id = config.Id});
        }
    }
}