using FreeRentLibrary.Data;
using FreeRentLibrary.Helpers;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


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

        // GET: Books
        public IActionResult Index()
        {
            return View(_bookRepository.GetAll().OrderBy(p => p.Title));
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

        // GET: Books/Create
        //[Authorize(Roles ="Admin")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _bookRepository.CreateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //private Book ToBook(BookViewModel model, string path)
        //{
        //    return new Book
        //    {
        //        Id = model.Id,
        //        ImageUrl = path,
        //        IsAvailable = model.IsAvailable,
        //        LastPurchase = model.LastPurchase,
        //        LastSale = model.LastSale,
        //        Name = model.Name,
        //        Price = model.Price,
        //        Stock = model.Stock,
        //        User = model.User
        //    };
        //}

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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                    book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

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

        //private BookViewModel ToBookViewModel(Book Book)
        //{
        //    return new BookViewModel
        //    {
        //        Id = book.Id,
        //        IsAvailable = book.IsAvailable,
        //        LastPurchase = book.LastPurchase,
        //        LastSale = book.LastSale,
        //        ImageUrl = book.ImageUrl,
        //        Price = book.Price,
        //        Name = book.Name,
        //        Stock = book.Stock,
        //        User = book.User
        //    };
        //}

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
                    ViewBag.ErrorTitle = $"{book.Title} provavelmente esta a ser usado!!";
                    ViewBag.ErrorMessage = $"{book.Title} não pode ser apagado visto que ha encomenda que o usam.</br></br>" +
                    $"Experimente apagar todas as encomendas que estão a usar," +
                    $"e torne novamente a apaga-lo";

                }
                return View("Error");
            }


        }

        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}
