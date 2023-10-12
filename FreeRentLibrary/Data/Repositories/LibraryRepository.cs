using FreeRentLibrary.Controllers;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories
{
	public class LibraryRepository : GenericRepository<Book>, ILibraryRepository
	{
		private readonly DataContext _context;
		private readonly RentRepository _rentRepository;
		private readonly ReserveRepository _reserveRepository;

		public LibraryRepository(DataContext context, RentRepository rentRepository, ReserveRepository reserveRepository): base(context)
		{
			_context = context;
			_rentRepository = rentRepository;
			_reserveRepository = reserveRepository;
		}

		public async Task<Book> CheckAndReserveBookAsync(int libraryId, int bookId, string userId)
		{
			var library = await _context.Libraries.FindAsync(libraryId);
			if (library == null) // Library not Found
			{
				return null;
			}

			var stock = library.LibraryStocks.FirstOrDefault(s => s.BookEditionId == bookId);
			if (stock == null) // The book is not in stock in this library
			{
				await _reserveRepository.ReserveBookAsync(userId, libraryId, bookId);

				return null;
			}

			var bookAvailableId = Convert.ToInt32(stock.BookEditionId);
			try
			{
				await _rentRepository.RentBookAsync(userId,libraryId,bookAvailableId);
			}
			catch (Exception e)
			{
				throw;
			}

			return null;
		}

		public IEnumerable<SelectListItem> GetUserBooks()
		{
			throw new System.NotImplementedException();
		}

		public IQueryable GetUserLibrary()
		{
			throw new System.NotImplementedException();
		}

		/*public IQueryable GetUserLibrary()
        {

           return _context.Books.Include(b => b.User);
        }

        public IEnumerable<SelectListItem> GetUserBooks()
        {
           var list = _context.Books.Select(p => new SelectListItem
           {
               Text = p.Title,
               Value = p.Id.ToString()

           }).ToList();
           list.Insert(0, new SelectListItem
           {
               Text = "(Select a product...)",
               Value = "0"
           });
           return list;
        }*/

	}
}
