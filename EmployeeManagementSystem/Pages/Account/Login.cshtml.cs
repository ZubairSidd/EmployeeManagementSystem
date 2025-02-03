using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(IUserService userService, ILogger<LoginModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IActionResult OnGet(string returnUrl = null)
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToPage("/Account/Profile");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _userService.AuthenticateAsync(Input.Email, Input.Password);

                if (!result.success)
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }

                HttpContext.Session.SetInt32("UserId", result.user.Id);
                _logger.LogInformation("User {Email} logged in successfully", Input.Email);


                if (result.user.IsAdmin)
                    return RedirectToPage("/Admin/Index");
                else
                    return RedirectToPage("/Account/Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt for user {Email}", Input.Email);
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                return Page();
            }
        }
    }
}
