using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.Interfaces;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly IUserProfileService _profileService;
        private readonly ILogger<ProfileModel> _logger;

        [BindProperty]
        public ProfileInputModel Input { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; }

        public ProfileModel(IUserProfileService profileService, ILogger<ProfileModel> logger)
        {
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

            try
            {
                IsAdmin = await _profileService.IsAdminAsync(userId.Value);
                var profile = await _profileService.GetProfileAsync(userId.Value);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile for user {UserId}", userId);
                TempData["ErrorMessage"] = "An error occurred while loading your profile.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!string.IsNullOrEmpty(Input.ExistingProfileImage) && Input.ProfilePicture == null)
            {
                ModelState.Remove("Input.ProfilePicture");
            }
            ModelState.Remove("Input.ExistingProfileImage");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var profile = await _profileService.GetProfileAsync(userId.Value);
                if (profile == null)
                {
                    return NotFound();
                }

                // Update profile properties
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
                    if (Input.ProfilePicture.Length > 5 * 1024 * 1024) // 5MB limit
                    {
                        ModelState.AddModelError("Input.ProfilePicture", "File size should not exceed 5MB.");
                        return Page();
                    }

                    using var memoryStream = new MemoryStream();
                    await Input.ProfilePicture.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    profile.ProfileImage = Convert.ToBase64String(imageBytes);
                }

                var result = await _profileService.UpdateProfileAsync(profile);

                if (!result.success)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user {UserId}", userId);
                ModelState.AddModelError(string.Empty, "An error occurred while updating your profile. Please try again.");
                return Page();
            }
        }

       
    }
}
