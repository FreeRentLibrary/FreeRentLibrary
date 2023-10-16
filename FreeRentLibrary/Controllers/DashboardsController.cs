using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRentLibrary.Controllers
{
    public class DashboardsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public DashboardsController(ILogger<HomeController> logger, DataContext dataContext, IUserHelper userHelper)
        {
            _logger = logger;
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            DashboardViewModel dashboard = new DashboardViewModel();

            if (User.IsInRole("Admin"))
            {
                dashboard.AdminDashboard = new AdminDashboardViewModel();
                dashboard.AdminDashboard.Readers = new List<User>();
                dashboard.AdminDashboard.Employees = new List<User>();

                foreach (var user in _dataContext.Users)
                {
                    if (await _userHelper.IsUserInRoleAsync(user, "Reader")) //user.EmployeeApproved && !user.AdminApproved && 
                    {
                        dashboard.AdminDashboard.Readers.Add(user);
                    }

                    if (await _userHelper.IsUserInRoleAsync(user, "Employee")) //!user.AdminApproved &&
                    {
                        dashboard.AdminDashboard.Employees.Add(user);
                    }

                }
            }
            else if (User.IsInRole("Employee"))
            {
                dashboard.EmployeeDashboard = new EmployeeDashboardViewModel();
                dashboard.EmployeeDashboard.Readers = new List<User>();

                foreach (var user in _dataContext.Users)
                {
                    if (await _userHelper.IsUserInRoleAsync(user, "Reader")) //!user.EmployeeApproved && 
                    {
                        dashboard.EmployeeDashboard.Readers.Add(user);
                    }
                }

            }
            else if (User.IsInRole("Reader"))
            {

            }


            return View(dashboard);
        }
    }
}
