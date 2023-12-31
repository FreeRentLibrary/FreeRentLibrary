﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace FreeRentLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public BooksController(IBookRepository bookRepository, 
            IGenreRepository genreRepository, 
            IAuthorRepository authorRepository, 
            IConverterHelper converterHelper,
            IBlobHelper blobHelper)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
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
            var viewModel = new BookViewModel
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
        public async Task<IActionResult> Create(BookViewModel viewModel)
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
            var viewModel = _converterHelper.ToBookViewModel(book);
            viewModel.Genres = _genreRepository.GetAll();
            viewModel.Authors = _authorRepository.GetComboAuthors(viewModel.AuthorId);
            return View(viewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (viewModel.SelectedGenres == null || viewModel.SelectedGenres.Count == 0)
                    {
                        viewModel.Genres = _genreRepository.GetAll();
                        return View(viewModel);
                    }

                    var existingBook = await _bookRepository.GetBookWithAllDataAsync(viewModel.Id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    existingBook.Name = viewModel.Name;
                    existingBook.Synopsis = viewModel.Synopsis;
                    existingBook.NativeLanguage = viewModel.NativeLanguage;
                    existingBook.AuthorId = viewModel.AuthorId;

                    var genres = _genreRepository.GetGenres(viewModel.SelectedGenres);

                    existingBook.BookGenres = genres.Select(genre => new BookGenre
                    {
                        Book = existingBook,
                        Genre = genre
                    }).ToList();

                    await _bookRepository.UpdateAsync(existingBook);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.ExistAsync(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = viewModel.Id });
            }
            return View(viewModel);
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
            var viewModel = new BookEditionViewModel
            {
                BookId = book.Id,
                BookTypes = _bookRepository.GetComboBookTypes(),
                BookPublishers = _bookRepository.GetComboBookPublishers(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBookEdition(BookEditionViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(viewModel.ImageFile, "covers");
                    viewModel.CoverId = imageId;
                }


                await _bookRepository.AddBookEditionAsync(viewModel);
                return RedirectToAction("Details", new { id = viewModel.BookId });
            }
            return this.View(viewModel);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> EditBookEdition(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookEdition = await _bookRepository.GetBookEditionAsync(id.Value);
            if (bookEdition == null)
            {
                return NotFound();
            }

            var viewModel = _converterHelper.ToBookEditionViewModel(bookEdition);
            viewModel.BookTypes = _bookRepository.GetComboBookTypes(viewModel.BookTypeId);
            viewModel.BookPublishers = _bookRepository.GetComboBookPublishers(viewModel.BookPublisherId);
            return View(viewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBookEdition(BookEditionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(viewModel.ImageFile, "covers");
                        viewModel.CoverId = imageId;
                    }

                    var bookEdition = _converterHelper.ToBookEdition(viewModel);
                    await _bookRepository.UpdateBookEditionAsync(bookEdition);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.BookEditionExistsAsync(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("BookEditionsDetails", new { id = viewModel.Id });
            }
            return View(viewModel);
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
                BookTypes = _bookRepository.GetComboBookTypes(),
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
                Guid imageId = Guid.Empty;

                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(viewModel.ImageFile, "covers");
                }

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
                        BookViewModel bookViewModel = _converterHelper.ToBookViewModel(viewModel);
                        await _bookRepository.AddBookAsync(bookViewModel);
                        var book = await _bookRepository.GetBookWithNameAsync(bookViewModel.Name);
                        viewModel.BookId = book.Id;
                        BookEditionViewModel bookEditionViewModel = _converterHelper.ToBookEditionViewModel(viewModel, imageId);
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
                    BookEditionViewModel bookEdition = _converterHelper.ToBookEditionViewModel(viewModel, imageId);
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
            viewModel.BookTypes = _bookRepository.GetComboBookTypes();
            viewModel.BookPublisher = _bookRepository.GetComboBookPublishers();
            return viewModel;
        }
    }
}
