using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;

namespace FreeRentLibrary.Models
{
    public class DashboardViewModel
    {
        public AdminDashboardViewModel AdminDashboard { get; set; }
        public EmployeeDashboardViewModel EmployeeDashboard { get; set; }
        public ReaderDashboardViewModel ReaderDashboard { get; set; }

    }

    public class AdminDashboardViewModel
    {
        public List<User> Employees { get; set; }
        public List<User> Readers { get; set; }
        public User User { get; set; }
    }

    public class EmployeeDashboardViewModel
    {
        public List<User> Readers { get; set; }
        public User User { get; set; }
    }

    public class ReaderDashboardViewModel
    {
        
    }
}
