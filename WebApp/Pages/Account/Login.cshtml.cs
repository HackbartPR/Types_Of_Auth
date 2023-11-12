using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApp.Models;

namespace WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();
               
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Credential.UserName != "Admin" && Credential.Password != "123")
                return Page();

            List<Claim> claims = new()
            {
                new Claim("Admin", "true"),
                new Claim("ProbationDate", "2023-07-01"),
                new Claim("UserName", Credential.UserName),
            };

            ClaimsIdentity identity = new(claims, "AuthCookie");
            ClaimsPrincipal principal = new(identity);

            await HttpContext.SignInAsync("AuthCookie",principal, new AuthenticationProperties()
            {
                IsPersistent = Credential.RememberMe
            });

            return RedirectToPage("/Index");
        }
    }
}
