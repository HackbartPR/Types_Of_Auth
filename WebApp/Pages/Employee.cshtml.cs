using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    [Authorize(Policy = "Employee")]
    public class EmployeeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
