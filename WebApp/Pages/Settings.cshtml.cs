using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    [Authorize(Policy = "Admin")]
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
