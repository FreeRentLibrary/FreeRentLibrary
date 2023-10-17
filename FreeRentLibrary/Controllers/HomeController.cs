using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Repositories;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace FreeRentLibrary.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    return RedirectToAction("Dashboard", "Dashboards");
                }
                return RedirectToAction("Home");
            }
            return View();
        }

        public IActionResult Home()
        {
            var viewModel = new LibraryViewModel();
            try
            {
                viewModel.Books = _context.Books
                    .Include(b => b.BookEditions)
                    .Include(b => b.Author)
                    .Include(b => b.Genres)
                    .ToList();

                viewModel.BookOfTheDay = _context.Books
                    .FirstOrDefault()
                    .BookEditions
                    .FirstOrDefault();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
