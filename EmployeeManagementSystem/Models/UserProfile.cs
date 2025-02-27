﻿namespace EmployeeManagementSystem.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string? FirstName { get; set; }  
        public string? LastName { get; set; }   
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? EmployeeId { get; set; }
        public string? JobTitle { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
