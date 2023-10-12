using FreeRentLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
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

        public async Task<IEnumerable<Rent>> GetRentalsByUserAsync(string userId)
        {
            return await _context.Rentals.Include(r => r.Library)
                .ThenInclude(l => l.LibraryStocks)
                .ThenInclude(ls => ls.BookEdition)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task ReturnBookAsync(string userId, int libraryId)
        {
            var rent = await _context.Set<Rent>()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.LibraryId == libraryId && r.DueDate == null);

            if (rent != null)
            {
                rent.DueDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CancelRentalAsync(string userId, int libraryId)
        {
            var rent = await _context.Set<Rent>()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.LibraryId == libraryId && r.DueDate == null);

            if (rent != null)
            {
                _context.Set<Rent>().Remove(rent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RentBookAsync(string userId, int libraryId)
        {
            var rent = new Rent
            {
                UserId = userId,
                LibraryId = libraryId,
                RentDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30)
			};

            _context.Set<Rent>().Add(rent);
            await _context.SaveChangesAsync();
        }
    }
}
