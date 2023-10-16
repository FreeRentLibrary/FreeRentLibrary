using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Models;
using FreeRentLibrary.Data.Repositories;
using FreeRentLibrary.Helpers.IHelpers;

namespace FreeRentLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IConverterHelper _converterHelper;

        public BooksController(IBookRepository bookRepository, IGenreRepository genreRepository, IAuthorRepository authorRepository, IConverterHelper converterHelper)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
            _converterHelper = converterHelper;
        }

        // GET: Books
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_bookRepository.GetBooksWithAuthorsAndGenres());
        }

        // GET: Books/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetBookWithAllDataAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var viewModel = new AddBookViewModel
            {
                Genres = _genreRepository.GetAll(),
                Authors = _authorRepository.GetComboAuthors()
            };
            return View(viewModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.SelectedGenres == null || viewModel.SelectedGenres.Count == 0)
                {
                    viewModel.Genres = _genreRepository.GetAll();
                    viewModel.Authors = _authorRepository.GetComboAuthors();
                    return View(viewModel);
                }

                if (!_bookRepository.CheckIfBookExists(viewModel.Name, viewModel.AuthorId))
                {
                    await _bookRepository.AddBookAsync(viewModel);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "That book already exists in the database.");
                    viewModel.Genres = _genreRepository.GetAll();
                    viewModel.Authors = _authorRepository.GetComboAuthors();
                    return View(viewModel);
                }
            }
            viewModel.Genres = _genreRepository.GetAll();
            viewModel.Authors = _authorRepository.GetComboAuthors();
            return View(viewModel);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetBookWithAllDataAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _bookRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.ExistAsync(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookRepository.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }


        //-------------------- Book Editions -----------------------

        public async Task<IActionResult> AddBookEdition(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }
            var viewModel = new AddBookEditionViewModel
            {
                BookId = book.Id,
                BookType = _bookRepository.GetComboBookTypes(),
                BookPublisher = _bookRepository.GetComboBookPublishers(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBookEdition(AddBookEditionViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                await _bookRepository.AddBookEditionAsync(viewModel);
                return RedirectToAction("Details", new { id = viewModel.BookId });
            }
            return this.View(viewModel);
        }

        public async Task<IActionResult> BookEditionsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetBookEditionAsync(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult CreateBookEdition()
        {
            var viewModel = new BookAndBookEditionViewModel
            {
                Genres = _genreRepository.GetAll(),
                Authors = _authorRepository.GetComboAuthors(),
                Books = _bookRepository.GetComboBooks(),
                BookType = _bookRepository.GetComboBookTypes(),
                BookPublisher = _bookRepository.GetComboBookPublishers(),
            };
            return View(viewModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBookEdition(BookAndBookEditionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.CreateNewBook == true)
                {
                    if (viewModel.Name == null || viewModel.Name.Length == 0)
                    {
                        ModelState.AddModelError("Name", "Please write a name.");
                        viewModel = BookAndBookEditionPopulate(viewModel);
                        return View(viewModel);
                    }
                    if (viewModel.SelectedGenres == null || viewModel.SelectedGenres.Count == 0)
                    {
                        ModelState.AddModelError("SelectedGenres", "Please select at least one genre.");
                        viewModel = BookAndBookEditionPopulate(viewModel);
                        return View(viewModel);
                    }
                    if (viewModel.AuthorId.Value == null || viewModel.AuthorId.Value == 0)
                    {
                        ModelState.AddModelError("AuthorId", "You must select an Author.");
                        viewModel = BookAndBookEditionPopulate(viewModel);
                        return View(viewModel);
                    }
                    if (!_bookRepository.CheckIfBookExists(viewModel.Name, viewModel.AuthorId.Value))
                    {
                        AddBookViewModel bookViewModel = _converterHelper.ToBookViewModel(viewModel);
                        await _bookRepository.AddBookAsync(bookViewModel);
                        var book = await _bookRepository.GetBookWithNameAsync(bookViewModel.Name);
                        viewModel.BookId = book.Id;
                        AddBookEditionViewModel bookEditionViewModel = _converterHelper.ToBookEditionViewModel(viewModel);
                        await _bookRepository.AddBookEditionAsync(bookEditionViewModel);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "That book already exists in the database.");
                        viewModel = BookAndBookEditionPopulate(viewModel);
                        return View(viewModel);
                    }
                }
                else
                {
                    AddBookEditionViewModel bookEdition = _converterHelper.ToBookEditionViewModel(viewModel);
                    await _bookRepository.AddBookEditionAsync(bookEdition);
                    return RedirectToAction(nameof(Index));
                }
            }
            ModelState.AddModelError(string.Empty, "Something went wrong...");
            viewModel = BookAndBookEditionPopulate(viewModel);
            return View(viewModel);
        }

        public BookAndBookEditionViewModel BookAndBookEditionPopulate(BookAndBookEditionViewModel viewModel)
        {
            viewModel.Genres = _genreRepository.GetAll();
            viewModel.Authors = _authorRepository.GetComboAuthors();
            viewModel.Books = _bookRepository.GetComboBooks();
            viewModel.BookType = _bookRepository.GetComboBookTypes();
            viewModel.BookPublisher = _bookRepository.GetComboBookPublishers();
            return viewModel;
        }
    }
}
