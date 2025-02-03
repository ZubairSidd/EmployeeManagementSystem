namespace EmployeeManagementSystem.Models.Interfaces
{
    public interface IUserService
    {
        Task<(bool success, string message, User user)> RegisterUserAsync(string email, string password, bool isAdmin);
        Task<(bool success, string message, User user)> AuthenticateAsync(string email, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllEmployeesAsync();
        Task<bool> IsAdminAsync(int userId);
        Task DeleteUserAsync(int userId);
        Task<(bool success, string message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
