using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mobicon.Models;
using Mobicon.Services;

namespace Mobicon.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly ImportManager _importManager;
        private readonly DataContext _dataContext;

        public SimplePrefix[] Prefixes { get; set; }
        public Dictionary<string, bool[]> Roles { get; set; }

        public AdminModel(
            ImportManager importManager,
            DataContext dataContext)
        {
            _importManager = importManager;
            _dataContext = dataContext;
        }

        public void OnGet()
        {
            Prefixes = _dataContext.SimplePrefixes.ToArray();

            UserRole[] values = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToArray();
            Roles = _dataContext.UserRoles
                .ToDictionary(r => r.Username, r => values.Select(v => r.Role.HasFlag(v)).ToArray());
        }

        public IActionResult OnPostAddPrefix(string prefixName)
        {
            if (!_dataContext.SimplePrefixes.Any(p => p.Name == prefixName))
            {
                _dataContext.SimplePrefixes.Add(new SimplePrefix
                {
                    Name = prefixName
                });

                _dataContext.SaveChanges();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostImport(string zipUrl)
        {
            await _importManager.ImportFileStructure(zipUrl, User.Identity.Name);

            return RedirectToPage("Configs");
        }
    }
}