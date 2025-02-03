using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.Interfaces;

namespace EmployeeManagementSystem.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(
            IUserProfileRepository profileRepository,
            ILogger<UserProfileService> logger)
        {
            _profileRepository = profileRepository;
            _logger = logger;
        }

        public async Task<UserProfile> GetProfileAsync(int userId)
        {
            return await _profileRepository.GetByUserIdAsync(userId);
        }

        public async Task<(bool success, string message)> UpdateProfileAsync(UserProfile profile)
        {
            try
            {
                if (await IsEmployeeIdUniqueAsync(profile.EmployeeId, profile.UserId))
                {
                    await _profileRepository.UpdateAsync(profile);
                    await _profileRepository.SaveChangesAsync();
                    return (true, "Profile updated successfully");
                }
                return (false, "Employee ID already exists");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user {UserId}", profile.UserId);
                return (false, "An error occurred while updating the profile");
            }
        }

        public async Task<bool> IsEmployeeIdUniqueAsync(string employeeId, int excludeUserId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(excludeUserId);
            if (profile?.EmployeeId == employeeId)
            {
                return true;
            }
            return !await _profileRepository.EmployeeIdExistsAsync(employeeId);
        }
        public async Task<bool> IsAdminAsync(int userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            return profile?.User?.IsAdmin ?? false;
        }
    }
}
