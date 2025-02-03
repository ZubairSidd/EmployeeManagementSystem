namespace EmployeeManagementSystem.Models.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetProfileAsync(int userId);
        Task<(bool success, string message)> UpdateProfileAsync(UserProfile profile);
        Task<bool> IsEmployeeIdUniqueAsync(string employeeId, int excludeUserId);
        Task<bool> IsAdminAsync(int userId);
    }
}
