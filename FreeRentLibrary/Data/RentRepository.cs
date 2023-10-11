using FreeRentLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FreeRentLibrary.Data
{
    public class RentRepository : IRentRepository
    {
        private readonly DataContext _context;

        public RentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetRentalsByUserAsync(string userId)
        {
            return await _context.Set<Rent>()
                .Where(rent => rent.UserId == userId && rent.DueDate == null)
                .Select(rent => rent.Book)
                .ToListAsync();
        }

        public async Task ReturnBookAsync(string userId, int bookId)
        {
            var rent = await _context.Set<Rent>()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId && r.DueDate == null);

            if (rent != null)
            {
                rent.DueDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CancelRentalAsync(string userId, int bookId)
        {
            var rent = await _context.Set<Rent>()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId && r.DueDate == null);

            if (rent != null)
            {
                _context.Set<Rent>().Remove(rent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RentBookAsync(string userId, int bookId)
        {
            var rent = new Rent
            {
                UserId = userId,
                BookId = bookId,
                RentDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30)
			};

            _context.Set<Rent>().Add(rent);
            await _context.SaveChangesAsync();
        }
    }
}
