using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.Interfaces;

namespace EmployeeManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IUserProfileRepository profileRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<(bool success, string message, User user)> RegisterUserAsync(string email, string password, bool isAdmin = false)
        {
            if (await _userRepository.EmailExistsAsync(email))
            {
                return (false, "Email already exists", null);
            }

            var user = new User
            {
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(password),
                IsAdmin = isAdmin
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var profile = new UserProfile
            {
                UserId = user.Id
            };

            await _profileRepository.AddAsync(profile);
            await _profileRepository.SaveChangesAsync();

            return (true, "Registration successful", user);
        }

        public async Task<(bool success, string message, User user)> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return (false, "Invalid email or password", null);
            }

            return (true, "Authentication successful", user);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<User>> GetAllEmployeesAsync()
        {
            return await _userRepository.GetAllWithProfilesAsync();
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.IsAdmin ?? false;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
                await _userRepository.SaveChangesAsync();
            }
        }

        public async Task<(bool success, string message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            if (!_passwordHasher.VerifyPassword(currentPassword, user.PasswordHash))
                return (false, "Current password is incorrect.");

            user.PasswordHash = _passwordHasher.HashPassword(newPassword);
            await _userRepository.SaveChangesAsync();

            return (true, "Password changed successfully.");
        }
    }

}
