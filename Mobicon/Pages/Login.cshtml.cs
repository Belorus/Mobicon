using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mobicon.Auth;

namespace Mobicon.Pages
{
    public class LoginModel : PageModel
    {
        private readonly LdapAuthenticationService _authService;

        public LoginModel(LdapAuthenticationService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async void OnPost(string username, string password)
        {
            //var user = _authService.Login(username, password);
            var user = new ApplicationUser()
            {
                FullName = "Grigory Perepechko",
                UserName = "grigoryp"
            };

            if (user != null)
            {

                var claims = new List<Claim>
                {
                    new Claim("Name", user.UserName),
                    new Claim("FullName", user.FullName),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
                    "Name", "Role");

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties());
            }
        }
    }
}