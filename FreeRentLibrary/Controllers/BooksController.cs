using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Helpers.SimpleHelpers;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

//Ordered by CRUD -> Create(Add) > Read(Details) > Update(Edit) > Delete
namespace FreeRentLibrary.Controllers
{
    //[Authorize]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserHelper _userHelper;

        public readonly IBlobHelper _blobHelper;
        public readonly IConverterHelper _converterHelper;

        public BooksController(IBookRepository bookRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _bookRepository = bookRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Books/Create
        //[Authorize(Roles ="Admin")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "books");
                }

                var book = _converterHelper.ToBook(model, imageId, true);
                await _bookRepository.CreateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(_bookRepository.GetAll().OrderBy(b => b.Name));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        //[Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }
            var model = _converterHelper.ToBookViewModel(book);
            return View(model);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "books");
                    }

                    var book = _converterHelper.ToBook(model, imageId, false);

                    await _bookRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.ExistAsync(model.Id))
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
            return View(model);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }
            var model = _converterHelper.ToBookViewModel(book);
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            try
            {
                await _bookRepository.DeleteAsync(book);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{book.Name} provavelmente esta a ser usado!!";
                    ViewBag.ErrorMessage = $"{book.Name} não pode ser apagado visto está a ser alugado.</br></br>" +
                    $"Experimente cancelar todos os alugers," +
                    $"e torne novamente a apaga-lo";

                }
                return View("Error");
            }


        }

        public async Task<ActionResult> GetBooksByName(string name)
        {
            var books = await _bookRepository.GetBooksByNameAsync(name);

            if (books == null || !books.Any())
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(books);
        }


        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}
