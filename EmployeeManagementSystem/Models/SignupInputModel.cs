using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class SignupInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }

        public bool IsAdminRegistration { get; set; }

        [DataType(DataType.Password)]
        public string AdminCode { get; set; }
    }
}
