using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories;
using FreeRentLibrary.Data.Repositories.IRepositories;
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
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookPublisherRepository _publisherRepository;

        public HomeController(ILogger<HomeController> logger, 
            DataContext dataContext, 
            IBookRepository bookRepository, 
            IAuthorRepository authorRepository,
            IBookPublisherRepository publisherRepository)
        {
            _logger = logger;
            _context = dataContext;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                //{
                //    return RedirectToAction("Dashboard", "Dashboards");
                //}
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
                    .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
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


        public async Task<IActionResult> SearchResults(string category, string query)
        {
            var viewModel = new SearchResultsViewModel();
            switch (category)
            {
                case "BookEdition":
                    viewModel.BookEditionResults = await _bookRepository.SearchBookEditionsAsync(query);
                    break;
                case "Author":
                    viewModel.AuthorResults = await _authorRepository.SearchAuthorAsync(query);
                    break;
                case "Publisher":
                    viewModel.PublisherResults = await _publisherRepository.SearchBookPublisherAsync(query);
                    break;
                default:
                    return View("Error");
            }
            return View(viewModel);
        }
    }
}
