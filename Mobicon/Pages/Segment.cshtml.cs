using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mobicon.Models;

namespace Mobicon.Pages
{
    [Authorize]
    public class SegmentModel : PageModel
    {
        private readonly DataContext _dataContext;

        public Config[] Configs { get; private set; }

        public SegmentModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void OnGet(int id)
        {
            Configs = _dataContext.Configs.Where(c => c.SegmentId == id).ToArray();
        }

        public IActionResult OnPostImport(int id, string data)
        {
            return null;
        }

        public IActionResult OnPost(int id, string configName)
        {
            _dataContext.Configs.Add(new Config()
            {
                Name = configName,
                SegmentId = id,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name
            });

            _dataContext.SaveChanges();

            return RedirectToPage(new {id = id});
        }
    }
}