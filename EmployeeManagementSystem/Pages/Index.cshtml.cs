using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToPage("/Account/Profile");
            }
            return RedirectToPage("/Account/Login");
        }
    }
}
