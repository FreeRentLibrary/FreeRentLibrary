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

namespace FreeRentLibrary.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;

        public AuthorsController(IAuthorRepository authorRepository, IGenreRepository genreRepository)
        {
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
        }

        // GET: Authors
        public IActionResult Index()
        {
            return View(_authorRepository.GetAll());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorRepository.GetAuthorWithGenresAndBooks(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            var viewModel = new AuthorViewModel
            {
                Genres = _genreRepository.GetAll()
            };

            return View(viewModel);
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.SelectedGenres == null || viewModel.SelectedGenres.Count == 0)
                {
                    viewModel.Genres = _genreRepository.GetAll();
                    return View(viewModel);
                }
                await _authorRepository.AddAuthorWithGenresAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }
            viewModel.Genres = _genreRepository.GetAll();
            return View(viewModel);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorRepository.GetAuthorWithGenresAndBooks(id.Value);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _authorRepository.UpdateAsync(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _authorRepository.ExistAsync(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorRepository.GetByIdAsync(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            await _authorRepository.DeleteAsync(author);
            return RedirectToAction(nameof(Index));
        }

    }
}
