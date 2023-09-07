using Microsoft.AspNetCore.Identity;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Customer");


            // Creates Admin
            var user = await _userHelper.GetUserByEmailAsync("luispatricio.info@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Luis",
                    LastName = "Patricio",
                    Email = "luispatricio.info@gmail.com",
                    UserName = "luispatricio.info@gmail.com",
                    PhoneNumber = "999666333",
                    Address = "Rua Ua 2",
                    EmployeeApproved = true,
                    AdminApproved = true
                };

                var result = await _userHelper.AddUserAsync(user, "121212");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not Create User in Seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            //Creates Employee
            user = await _userHelper.GetUserByEmailAsync("randomEmployee.info@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Mister",
                    LastName = "Employee",
                    Email = "randomEmployee.info@gmail.com",
                    UserName = "randomEmployee.info@gmail.com",
                    PhoneNumber = "321123456",
                    Address = "Rua AUa 3",
                    EmployeeApproved = true,
                    AdminApproved = true
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not Create User in Seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Employee");
            }

            isInRole = await _userHelper.IsUserInRoleAsync(user, "Employee");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Employee");
            }

            await _context.SaveChangesAsync();
        }
    }
}
