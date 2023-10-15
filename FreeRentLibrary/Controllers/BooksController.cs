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

namespace FreeRentLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;

        public BooksController(IBookRepository bookRepository, IGenreRepository genreRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(_bookRepository.GetBooksWithAuthorsAndGenres());
        }

        // GET: Books/Details/5
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

                if (!_bookRepository.CheckIfBookExists(viewModel))
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
            var book = await _bookRepository.GetByIdAsync(id);
            await _bookRepository.DeleteAsync(book);
            return RedirectToAction(nameof(Index));
        }
    }
}
