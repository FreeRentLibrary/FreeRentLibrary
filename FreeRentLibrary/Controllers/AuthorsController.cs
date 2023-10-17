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
using FreeRentLibrary.Helpers.IHelpers;

namespace FreeRentLibrary.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public AuthorsController(IAuthorRepository authorRepository, 
            IGenreRepository genreRepository, 
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
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
            var viewModel = _converterHelper.ToAuthorViewModel(author);

            return View(viewModel);
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

                Guid imageId = Guid.Empty;

                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(viewModel.ImageFile, "authors");
                    viewModel.AuthorPhotoId = imageId;
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
            var viewModel = _converterHelper.ToAuthorViewModel(author);
            viewModel.Genres = _genreRepository.GetAll();
            return View(viewModel);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorViewModel viewModel)
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

                    var existingAuthor = await _authorRepository.GetAuthorWithGenresAndBooks(viewModel.Id);
                    if (existingAuthor == null)
                    {
                        return NotFound();
                    }

                    var genres = _genreRepository.GetGenres(viewModel.SelectedGenres);

                    Guid imageId = Guid.Empty;

                    if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(viewModel.ImageFile, "authors");
                        existingAuthor.AuthorPhotoId = imageId;
                    }

                    existingAuthor.AuthorGenres.Clear();
                    foreach (var genre in genres)
                    {
                        existingAuthor.AuthorGenres.Add(new AuthorGenre
                        {
                            Author = existingAuthor,
                            Genre = genre
                        });
                    }
                    
                    //var author = _converterHelper.ToAuthor(viewModel, genres);
                    await _authorRepository.UpdateAsync(existingAuthor);
                    //await _authorRepository.UpdateAuthorWithGenresAsync(viewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _authorRepository.ExistAsync(viewModel.Id))
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
            await _authorRepository.DeleteAllAuthorInfoAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
