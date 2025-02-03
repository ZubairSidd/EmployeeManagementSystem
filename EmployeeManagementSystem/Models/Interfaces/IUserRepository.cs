namespace EmployeeManagementSystem.Models.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<User>> GetAllWithProfilesAsync();
    }
}
