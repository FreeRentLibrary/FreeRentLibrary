﻿using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IUserHelper
    {
        
        #region UserRegion
        
        Task<User> GetUserByIdAsync(string id);
        
        Task<User> GetUserByEmailAsync(string email);
        
        Task<IdentityResult> AddUserAsync(User user, string password);
        
        Task<IdentityResult> UpdateUserAsync(User user);
        
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        
        #endregion

        //--

        #region SignInRegion
        
        Task<SignInResult> LoginAsync(LoginViewModel model);
        
        Task LogoutAsync();
        
        #endregion
        
        //--
        
        #region RoleRegion
        
        Task<bool> CheckRoleAsync(string roleName);
        
        Task AddUserToRoleAsync(User user, string roleName);
        
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<System.Linq.IQueryable<IdentityRole>> GetRoles();

        #endregion

        //--

        #region ValidationRegion

        Task<SignInResult> ValidatePasswordAsync(User user, string password);
        
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        
        Task<string> GeneratePasswordResetTokenAsync(User user);
        
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
        
        Task<string> GenerateTwoFactorAuthenticationTokenAsync(User user);
        
        Task<IdentityResult> TurnTwoFactorAuthenticationOnAsync(User user);
        
        Task<bool> TwoFactorAuthenticationConfirmationAsync(User user, string token);
        
        #endregion

        //SAMPLE TO DELETE LATER
        IQueryable GetBookEditionsReservedByUser(string userId);
    }
}
