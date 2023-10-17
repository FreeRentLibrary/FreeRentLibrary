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
    public class BookPublishersController : Controller
    {
        private readonly IBookPublisherRepository _publisherRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IConverterHelper _converterHelper;

        public BookPublishersController(IBookPublisherRepository publisherRepository, ICountryRepository countryRepository, IConverterHelper converterHelper)
        {
            _publisherRepository = publisherRepository;
            _countryRepository = countryRepository;
            _converterHelper = converterHelper;
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
            var viewModel = new BookPublisherViewModel
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
        public async Task<IActionResult> Create(BookPublisherViewModel viewModel)
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
            var viewModel = _converterHelper.ToBookPublisherViewModel(bookPublisher);
            viewModel.Countries = _countryRepository.GetComboCountries(bookPublisher.CountryId);
            return View(viewModel);
        }

        // POST: BookPublishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookPublisherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingPublisher = await _publisherRepository.GetPublisherWithBooksAndCountry(viewModel.Id);
                    if (existingPublisher == null)
                    {
                        return NotFound();
                    }

                    var country = await _countryRepository.GetCountryWithCityAsync(viewModel.CountryId);

                    existingPublisher.Name = viewModel.Name;
                    existingPublisher.CountryId = viewModel.CountryId;
                    existingPublisher.Country = country;

                    await _publisherRepository.UpdateAsync(existingPublisher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _publisherRepository.ExistAsync(viewModel.Id))
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
            return View(viewModel);
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
