using EmployeeManagementSystem.Models.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data.Repositories
{
    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserProfile> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<bool> EmployeeIdExistsAsync(string employeeId)
        {
            return await _dbSet.AnyAsync(p => p.EmployeeId == employeeId);
        }
    }
}
