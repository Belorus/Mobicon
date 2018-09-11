﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mobicon.Auth;

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
                return RedirectToPage("/Segments");
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
            //var user = _authService.Login(username, password);
            var user = new ApplicationUser()
            {
                FullName = "Grigory Perepechko",
                UserName = "grigoryp"
            };

            if (user != null)
            {

                var role = _dataContext.UserRoles
                    .Find(user.UserName)
                    .Role.ToString();

                var claims = new List<Claim>
                {
                    new Claim("Name", user.UserName),
                    new Claim("FullName", user.FullName),
                    new Claim("Role", role),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
                    "Name", "Role");

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties());

                return RedirectToPage("/Segments");
            }
            else
            {
                return Page();
            }
        }
    }
}