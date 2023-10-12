using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region UserRegion

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        #endregion

        //--

        #region SignInRegion

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        //--

        #region RoleRegion

        public async Task<System.Linq.IQueryable<IdentityRole>> GetRoles()
        {
           return _roleManager.Roles;
        }

        public async Task<bool> CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
                return true;
            }
            return false;
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        #endregion

        //--

        #region ValidationRegion

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<string> GenerateTwoFactorTokenAsync(User user)
        {
            return await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
        }

        public async Task<IdentityResult> TurnTwofactorOn(User user)
        {
            return await _userManager.SetTwoFactorEnabledAsync(user, true);
        }

        public async Task<bool> TwoFactorConfirmation(User user, string token)
        {
            return await _userManager.VerifyTwoFactorTokenAsync(user, "Email", token);
        }

        #endregion

        //SAMPLE TO DELETE LATER
        public IQueryable GetBookEditionsReservedByUser(string userId)
        {
            //return await _userManager.Users.Include(u => u.Reservations)
            //    .ThenInclude(r => r.Library)
            //    .ThenInclude(l => l.LibraryStocks)
            //    .ThenInclude(ls => ls.BookEdition)
            //    .Where(u => u.Id == userId)
            //    .ToListAsync();

            return _userManager.Users.Include(u => u.Reservations)
                .ThenInclude(r => r.BookEdition)
                .Where(u => u.Id == userId);
        }
    }
}
