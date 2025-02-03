using EmployeeManagementSystem.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserProfileService _profileService;
        private readonly ILogger<IndexModel> _logger;

        public IList<EmployeeViewModel> Employees { get; set; }

        public class EmployeeViewModel
        {
            public int UserId { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
            public string EmployeeId { get; set; }
            public string JobTitle { get; set; }
            public DateTime? DateOfBirth { get; set; }
        }

        public IndexModel(
            IUserService userService,
            IUserProfileService profileService,
            ILogger<IndexModel> logger)
        {
            _userService = userService;
            _profileService = profileService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            var currentUser = await _userService.GetUserByIdAsync(userId.Value);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var employees = await _userService.GetAllEmployeesAsync();
                Employees = employees.Select(e => new EmployeeViewModel
                {
                    UserId = e.Id,
                    Email = e.Email,
                    FullName = $"{e.Profile?.FirstName} {e.Profile?.LastName}",
                    EmployeeId = e.Profile?.EmployeeId,
                    JobTitle = e.Profile?.JobTitle,
                    DateOfBirth = e.Profile?.DateOfBirth
                }).ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employees list");
                TempData["ErrorMessage"] = "Error loading employees.";
                return Page();
            }
        }
    }
}
