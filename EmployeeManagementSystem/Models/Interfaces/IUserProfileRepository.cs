namespace EmployeeManagementSystem.Models.Interfaces
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile> GetByUserIdAsync(int userId);
        Task<bool> EmployeeIdExistsAsync(string employeeId);
    }
}
