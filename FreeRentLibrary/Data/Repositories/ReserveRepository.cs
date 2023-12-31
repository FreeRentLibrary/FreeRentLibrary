﻿using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FreeRentLibrary.Data.Repositories
{
    public class ReserveRepository : GenericRepository<Book>,IReserveRepository
    {
        private readonly DataContext _context;
        private readonly RentRepository _rentRepository;

        public ReserveRepository(DataContext context): base(context)
        {
            _context = context;
        }

        public async Task CancelReservationAsync(string userId, int libraryId)
        {
            var reserve = await _context.Set<Reservation>()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.LibraryId == libraryId && r.EndDate == null);

            reserve.EndDate = DateTime.Now;

            if (reserve != null)
            {
                _context.Set<Reservation>().Remove(reserve);
                await _context.SaveChangesAsync();
            }
        }

        //TODO: Correct
        public async Task<IEnumerable<Reservation>> GetReservationsByUserAsync(string userId)
        {
            return await _context.Reservations.Include(r => r.Library)
                .ThenInclude(l => l.LibraryStocks)
                .ThenInclude(ls => ls.BookEdition)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task ReserveBookAsync(string userId, int libraryId, int bookId)
        {
            var reservation = new Reservation
            {
                UserId = userId,
                LibraryId = libraryId,
                BookEditionId = bookId,
                ReservationDate = DateTime.Now
            };

            _context.Set<Reservation>().Add(reservation);
            await _context.SaveChangesAsync();
        }

    }
}
