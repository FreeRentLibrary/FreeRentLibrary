﻿using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Helpers.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Controllers
{
    [Authorize]
    public class LibrariesController : Controller
    {
        private readonly DataContext _context;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserHelper _userHelper;
		private readonly IRentRepository _rentRepository;
		private readonly IReserveRepository _reserveRepository;

		public LibrariesController(
            DataContext context, 
            ILibraryRepository libraryRepository, 
            IBookRepository bookRepository, 
            IUserHelper userHelper,
            IRentRepository rentRepository,
            IReserveRepository reserveRepository)
        {
            _context = context;
            _libraryRepository = libraryRepository;
            _bookRepository = bookRepository;
            _userHelper = userHelper;
			_rentRepository = rentRepository;
			_reserveRepository = reserveRepository;
		}

        // GET: Libraries
        public async Task<IActionResult> Index()
        {
            return View(_bookRepository.GetAll().OrderBy(b => b.Name));
        }

        // GET: Libraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.Libraries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // GET: Libraries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Library library)
        {
            if (ModelState.IsValid)
            {
                _context.Add(library);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(library);
        }

        // GET: Libraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.Libraries.FindAsync(id);
            if (library == null)
            {
                return NotFound();
            }
            return View(library);
        }

        // POST: Libraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Library library)
        {
            if (id != library.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(library);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryExists(library.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(library);
        }

        // GET: Libraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.Libraries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // POST: Libraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var library = await _context.Libraries.FindAsync(id);
            _context.Libraries.Remove(library);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryExists(int id)
        {
            return _context.Libraries.Any(e => e.Id == id);
        }

		public async Task<IActionResult> RentOrReserve(int libraryId, int bookId, string userId)
		{
			var library = await _context.Libraries.FindAsync(libraryId);
			if (library == null) // Library not Found
			{
				return NotFound();
			}

			var stock = library.LibraryStocks.FirstOrDefault(s => s.BookEditionId == bookId);
			if (stock == null) // The book is not in stock in this library
			{
				await _reserveRepository.ReserveBookAsync(userId, libraryId, bookId);

				return View("UserReservations"); //Returns to the User's Reservations Page
			}

			var bookAvailableId = Convert.ToInt32(stock.BookEditionId);
			try
			{
				await _rentRepository.RentBookAsync(userId, libraryId, bookAvailableId);
			}
			catch (Exception e)
			{
				return NotFound(e);
			}

            return View("UserRentals"); //Returns to the User's Rentals Page
		}
	}
}