using EmployeeManagementSystem.Models.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserProfileService _profileService;
        private readonly ILogger<EditModel> _logger;

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        public EditModel(
            IUserService userService,
            IUserProfileService profileService,
            ILogger<EditModel> logger)
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
            if (!currentUserId.HasValue || !await _userService.IsAdminAsync(currentUserId.Value))
            {
                return RedirectToPage("/Account/Login");
            }

            var profile = await _profileService.GetProfileAsync(id.Value);
            if (profile == null)
            {
                return NotFound();
            }

            Input = new ProfileInputModel
            {
                FirstName = profile.FirstName,
                MiddleName = profile.MiddleName,
                LastName = profile.LastName,
                DateOfBirth = profile.DateOfBirth ?? DateTime.Today,
                EmployeeId = profile.EmployeeId,
                JobTitle = profile.JobTitle,
                ExistingProfileImage = profile.ProfileImage
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ModelState.Remove("Input.ProfilePicture");
            ModelState.Remove("Input.ExistingProfileImage");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (!currentUserId.HasValue || !await _userService.IsAdminAsync(currentUserId.Value))
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var profile = await _profileService.GetProfileAsync(id.Value);
                if (profile == null)
                {
                    return NotFound();
                }

                // Update profile
                profile.FirstName = Input.FirstName;
                profile.MiddleName = Input.MiddleName;
                profile.LastName = Input.LastName;
                profile.DateOfBirth = DateTime.SpecifyKind(Input.DateOfBirth, DateTimeKind.Utc);
                profile.EmployeeId = Input.EmployeeId;
                profile.JobTitle = Input.JobTitle;
                profile.UpdatedAt = DateTime.UtcNow;

                // Handle profile picture
                if (Input.ProfilePicture != null && Input.ProfilePicture.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Input.ProfilePicture.CopyToAsync(memoryStream);
                    profile.ProfileImage = Convert.ToBase64String(memoryStream.ToArray());
                }

                var result = await _profileService.UpdateProfileAsync(profile);
                if (!result.success)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }

                TempData["SuccessMessage"] = "Employee updated successfully.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee {EmployeeId}", id);
                ModelState.AddModelError(string.Empty, "Error updating employee.");
                return Page();
            }
        }
    }
}
