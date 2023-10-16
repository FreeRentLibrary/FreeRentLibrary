using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;

namespace FreeRentLibrary.Controllers
{
    public class RentsController : Controller
    {
        private readonly DataContext _context;

        public RentsController(DataContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Rentals.Include(r => r.BookEdition).Include(r => r.Library).Include(r => r.User);
            return View(await dataContext.ToListAsync());
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rentals
                .Include(r => r.BookEdition)
                .Include(r => r.Library)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // GET: Rents/Create
        public IActionResult Create()
        {
            ViewData["BookEditionId"] = new SelectList(_context.BookEditions, "Id", "Id");
            ViewData["LibraryId"] = new SelectList(_context.Libraries, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RentDate,DueDate,LibraryId,UserId,BookEditionId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookEditionId"] = new SelectList(_context.BookEditions, "Id", "Id", rent.BookEditionId);
            ViewData["LibraryId"] = new SelectList(_context.Libraries, "Id", "Id", rent.LibraryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rent.UserId);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rentals.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            ViewData["BookEditionId"] = new SelectList(_context.BookEditions, "Id", "Id", rent.BookEditionId);
            ViewData["LibraryId"] = new SelectList(_context.Libraries, "Id", "Id", rent.LibraryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rent.UserId);
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RentDate,DueDate,LibraryId,UserId,BookEditionId")] Rent rent)
        {
            if (id != rent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.Id))
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
            ViewData["BookEditionId"] = new SelectList(_context.BookEditions, "Id", "Id", rent.BookEditionId);
            ViewData["LibraryId"] = new SelectList(_context.Libraries, "Id", "Id", rent.LibraryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rent.UserId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rentals
                .Include(r => r.BookEdition)
                .Include(r => r.Library)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }

        

    }
}
