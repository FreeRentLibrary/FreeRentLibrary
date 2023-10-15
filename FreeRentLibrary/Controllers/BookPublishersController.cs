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
    public class BookPublishersController : Controller
    {
        private readonly IBookPublisherRepository _publisherRepository;
        private readonly ICountryRepository _countryRepository;

        public BookPublishersController(IBookPublisherRepository publisherRepository, ICountryRepository countryRepository)
        {
            _publisherRepository = publisherRepository;
            _countryRepository = countryRepository;
        }

        // GET: BookPublishers
        public IActionResult Index()
        {
            return View(_publisherRepository.GetAll());
        }

        // GET: BookPublishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPublisher = await _publisherRepository.GetPublisherWithBooksAndCountry(id.Value);
            if (bookPublisher == null)
            {
                return NotFound();
            }

            return View(bookPublisher);
        }

        // GET: BookPublishers/Create
        public IActionResult Create()
        {
            var viewModel = new AddBookPublisherViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
            };
            return View(viewModel);
        }

        // POST: BookPublishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBookPublisherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var publisher = await _publisherRepository.GetPublisherWithNameAsync(viewModel.Name);
                if (publisher == null)
                {
                    var newPublisher = await _publisherRepository.AddBookPublisherWithCountry(viewModel);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There already exists a Publisher with that name.");
                    viewModel.Countries = _countryRepository.GetComboCountries();
                    return View(viewModel);
                }
            }
            viewModel.Countries = _countryRepository.GetComboCountries();
            return View(viewModel);
        }

        // GET: BookPublishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPublisher = await _publisherRepository.GetPublisherWithBooksAndCountry(id.Value);
            if (bookPublisher == null)
            {
                return NotFound();
            }
            return View(bookPublisher);
        }

        // POST: BookPublishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookPublisher bookPublisher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _publisherRepository.UpdateAsync(bookPublisher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _publisherRepository.ExistAsync(bookPublisher.Id))
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
            return View(bookPublisher);
        }

        // GET: BookPublishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPublisher = await _publisherRepository.GetByIdAsync(id.Value);
            if (bookPublisher == null)
            {
                return NotFound();
            }

            return View(bookPublisher);
        }

        // POST: BookPublishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookPublisher = await _publisherRepository.GetByIdAsync(id);
            await _publisherRepository.DeleteAsync(bookPublisher);
            return RedirectToAction(nameof(Index));
        }
    }
}
