using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mobicon.Auth;
using Mobicon.Models;

namespace Mobicon.Pages
{
    public class LoginModel : PageModel
    {
        private readonly DataContext _dataContext;
        private readonly LdapAuthenticationService _authService;

        public LoginModel(
            DataContext dataContext,
            LdapAuthenticationService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            if (User.Claims.Any())
            {
                return RedirectToPage("/Configs");
            }
            else
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPost(string username, string password)
        {
            var user = _authService.Login(username, password);
            if (user != null)
            {
                var userRoles = _dataContext.UserRoles
                    .Where(u => u.Username == username)
                    .ToArray();


                var claims = new List<Claim>
                {
                    new Claim("Name", user.UserName),
                    new Claim("FullName", user.FullName),
                    new Claim("Role", UserRole.Reader.ToString()) // Everyone should be reader by default
                };

                foreach (var role in userRoles)
                {
                    claims.Add(new Claim("Role", role.Role.ToString()));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
                    "Name", "Role");

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties());

                return RedirectToPage("/Configs");
            }
            else
            {
                return Page();
            }
        }
    }
}