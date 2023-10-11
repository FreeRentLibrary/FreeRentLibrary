using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FreeRentLibrary.Data
{
	public class ReserveRepository : IReserveRepository
    {
        private readonly DataContext _context;
		private readonly RentRepository _rentRepository;

		public ReserveRepository(DataContext context, RentRepository rentRepository)
        {
            _context = context;
			_rentRepository = rentRepository;
		}

		public async Task CancelReservationAsync(string userId, int bookId)
		{
			var reserve = await _context.Set<Reservation>()
				.FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId && r.EndDate == null);
			
			reserve.EndDate = DateTime.Now;

			if (reserve != null)
			{
				_context.Set<Reservation>().Remove(reserve);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Book>> GetReservationsByUserAsync(string userId)
		{
			return await _context.Set<Reservation>()
				.Where(reservation => reservation.UserId == userId && reservation.EndDate == null)
				.Select(reservation => reservation.Book)
				.ToListAsync();
		}

		public async Task ReserveBookAsync(string userId, int bookId)
		{
			var reservation = new Reservation
			{
				UserId = userId,
				BookId = bookId,
				ReservationDate = DateTime.Now
			};

			_context.Set<Reservation>().Add(reservation);
			await _context.SaveChangesAsync();
		}

		public async Task ReserveToRentAsync(Reservation reservation)
		{
			if (reservation.UserId != null && reservation.BookId != null)
			{
				var userId = reservation.UserId;
				int bookId = reservation.BookId.Value;

				await _rentRepository.RentBookAsync(userId, bookId);

				reservation.EndDate = DateTime.Now;
				await _context.SaveChangesAsync();
			}
		}
	}
}
