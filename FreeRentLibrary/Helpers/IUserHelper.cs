using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers
{
    public interface IUserHelper
    {
        Task<System.Linq.IQueryable<IdentityRole>> GetRoles();
        Task<User> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<bool> CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string id);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
        Task<string> GenerateTwoFactorTokenAsync(User user);
        Task<IdentityResult> TurnTwofactorOn(User user);
        Task<bool> TwoFactorConfirmation(User user, string token);
    }
}
