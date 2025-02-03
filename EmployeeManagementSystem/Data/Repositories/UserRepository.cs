using EmployeeManagementSystem.Models.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }
        public async Task<IEnumerable<User>> GetAllWithProfilesAsync()
        {
            return await _dbSet
                .Include(u => u.Profile)
                .Where(u => !u.IsAdmin)
                .ToListAsync();
        }
    }
}
