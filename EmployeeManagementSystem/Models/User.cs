using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public UserProfile Profile { get; set; }

        public bool IsAdmin { get; set; }
    }
}
