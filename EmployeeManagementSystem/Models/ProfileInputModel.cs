using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class ProfileInputModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Middle Name cannot exceed 50 characters")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Employee ID is required")]
        [Display(Name = "Employee ID")]
        [StringLength(20, ErrorMessage = "Employee ID cannot exceed 20 characters")]
        [RegularExpression(@"^[A-Za-z0-9-_]+$", ErrorMessage = "Employee ID can only contain letters, numbers, hyphens, and underscores")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Job Title is required")]
        [Display(Name = "Job Title")]
        [StringLength(100, ErrorMessage = "Job Title cannot exceed 100 characters")]
        public string JobTitle { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        public string ExistingProfileImage { get; set; }
    }
}
