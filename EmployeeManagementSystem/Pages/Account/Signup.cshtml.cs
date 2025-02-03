using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Pages.Account
{
    public class SignupModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<SignupModel> _logger;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public SignupInputModel Input { get; set; }

        public SignupModel(IUserService userService, ILogger<SignupModel> logger, IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToPage("/Account/Profile");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Input.IsAdminRegistration)
            {
                ModelState.Remove("Input.AdminCode");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (Input.IsAdminRegistration)
                {
                    var adminCode = _configuration["AdminSettings:RegistrationCode"];
                    if (string.IsNullOrEmpty(Input.AdminCode) || Input.AdminCode != adminCode)
                    {
                        ModelState.AddModelError("Input.AdminCode", "Invalid admin registration code.");
                        return Page();
                    }
                }

                var result = await _userService.RegisterUserAsync(Input.Email, Input.Password, Input.IsAdminRegistration);

                if (!result.success)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }

                HttpContext.Session.SetInt32("UserId", result.user.Id);
                return RedirectToPage(Input.IsAdminRegistration ? "/Admin/Index" : "/Account/Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Email}", Input.Email);
                ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                return Page();
            }
        }
    }
}
