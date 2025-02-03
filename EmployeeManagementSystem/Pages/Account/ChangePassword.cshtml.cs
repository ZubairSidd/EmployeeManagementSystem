using EmployeeManagementSystem.Models.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<ChangePasswordModel> _logger;

        [BindProperty]
        public ChangePasswordInputModel Input { get; set; }

        public ChangePasswordModel(IUserService userService, ILogger<ChangePasswordModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var result = await _userService.ChangePasswordAsync(
                    userId.Value,
                    Input.CurrentPassword,
                    Input.NewPassword);

                if (!result.success)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }

                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToPage("/Account/Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                ModelState.AddModelError(string.Empty, "An error occurred while changing your password.");
                return Page();
            }
        }
    }
}
