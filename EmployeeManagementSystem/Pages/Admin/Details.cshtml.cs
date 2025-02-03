using EmployeeManagementSystem.Models.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserProfileService _profileService;
        private readonly ILogger<DetailsModel> _logger;

        [BindProperty]
        public UserProfile Profile { get; set; }
        public User User { get; set; }

        public DetailsModel(
            IUserService userService,
            IUserProfileService profileService,
            ILogger<DetailsModel> logger)
        {
            _userService = userService;
            _profileService = profileService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (!currentUserId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            var currentUser = await _userService.GetUserByIdAsync(currentUserId.Value);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                return RedirectToPage("/Account/Login");
            }

            User = await _userService.GetUserByIdAsync(id.Value);
            Profile = await _profileService.GetProfileAsync(id.Value);

            if (User == null || Profile == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (!currentUserId.HasValue || !await _userService.IsAdminAsync(currentUserId.Value))
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                await _userService.DeleteUserAsync(id.Value);
                TempData["SuccessMessage"] = "Employee deleted successfully.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee {EmployeeId}", id);
                TempData["ErrorMessage"] = "Error deleting employee.";
                return RedirectToPage("./Index");
            }
        }
    }

}
