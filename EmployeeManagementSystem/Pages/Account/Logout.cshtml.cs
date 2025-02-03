using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPost()
        {
            _logger.LogInformation("User logged out.");
            HttpContext.Session.Clear();
            return RedirectToPage("./Login");
        }
    }
}
