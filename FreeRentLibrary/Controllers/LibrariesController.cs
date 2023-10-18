using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Helpers;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Models;
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
        private readonly IUserHelper _userHelper;
		private readonly IRentRepository _rentRepository;
		private readonly IReserveRepository _reserveRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICountryRepository _countryRepository;

        public LibrariesController(
            DataContext context, 
            ILibraryRepository libraryRepository, 
            IUserHelper userHelper,
            IRentRepository rentRepository,
            IReserveRepository reserveRepository,
            IBookRepository bookRepository,
            ICountryRepository countryRepository)
        {
            _context = context;
            _libraryRepository = libraryRepository;
            _userHelper = userHelper;
			_rentRepository = rentRepository;
			_reserveRepository = reserveRepository;
            _bookRepository = bookRepository;
            _countryRepository = countryRepository;
        }

        // GET: Libraries
        public IActionResult Index()
        {
            //Random rand = new Random();
            //int genreId = rand.Next(0,10); //Replace with _genreRepository.GetMaxGenreId();
            //int authorId = rand.Next(0,10); //Replace with _authorRepository.GetMaxAuthorId();
            //int bookDay = rand.Next(0, 10); //Set by Admin/Employee

            //var bookOfTheDay = await _bookRepository.GetBookEditionAsync(bookDay);
            //var booksByGenre = await _bookRepository.GetBooksByGenreAsync(genreId);
            //var booksByAuthor = await _bookRepository.GetBooksByAuthorAsync(authorId);

            //return View(new LibraryViewModel
            //{
            //    BookOfTheDay = bookOfTheDay,
            //    BooksByGenre = booksByGenre,
            //    BooksByAuthor = booksByAuthor,
            //});
            return View(_libraryRepository.GetLibrariesWithCity());
        }

        // GET: Libraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _libraryRepository.GetLibraryWithAllInfo(id.Value);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // GET: Libraries/Create
        public IActionResult Create()
        {
            var viewModel = new LibraryViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
            };
            return View(viewModel);
        }

        // POST: Libraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibraryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var library = new Library
                {
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    CityId = viewModel.CityId,
                    City = await _countryRepository.GetCityAsync(viewModel.CityId),
                };

                await _libraryRepository.CreateAsync(library);
                return RedirectToAction(nameof(Index));
            }

            viewModel.Countries = _countryRepository.GetComboCountries(viewModel.CountryId);
            viewModel.Cities = _countryRepository.GetComboCities(viewModel.CityId);
            return View(viewModel);
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

        public async Task<IActionResult> AddLibraryStock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _libraryRepository.GetByIdAsync(id.Value);

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            var viewModel = new LibraryStockViewModel
            {
                LibraryId = id.Value,
                BookEditions = _bookRepository.GetComboBookEditions(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddLibraryStock(LibraryStockViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                await _libraryRepository.AddBookToStock(viewModel.BookEditionId.Value, viewModel.LibraryId.Value, viewModel.Quantity);
                
                return RedirectToAction("Details", new { id = viewModel.LibraryId.Value });
            }
            return this.View(viewModel);
        }

    }
}
