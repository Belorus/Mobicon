using System;
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

        public async Task<IActionResult> OnPost(string username, string password, string returnUrl)
        {
            var user = _authService.Login(username, password);
            if (user != null)
            {
                var userRole = _dataContext.UserRoles
                    .FirstOrDefault(u => u.Username == username);

                if (userRole == null)
                {
                    userRole = new UserToRole()
                        {
                            Role = UserRole.Reader,
                            Username = username
                        };
                    _dataContext.UserRoles.Add(userRole);
                    _dataContext.SaveChanges();
                }

                var claims = new List<Claim>
                {
                    new Claim("Name", user.UserName),
                    new Claim("FullName", user.FullName),
                };

                foreach (var role in Enum.GetValues(typeof(UserRole)).Cast<UserRole>()
                    .Where(v => userRole.Role.HasFlag(v))
                    .Select(r => Enum.GetName(typeof(UserRole), r)))
                {
                    claims.Add(new Claim("Role", role));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
                    "Name", "Role");

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties());

                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToPage("/Configs");
                else
                    return RedirectToPage(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}