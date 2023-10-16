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

		public LibraryRepository(DataContext context): base(context)
		{
			_context = context;
		}

		public async Task<bool> CheckStockAsync(int libraryId, int bookId)
		{
			//var stock = await _context.LibraryStocks
			//	.FirstOrDefaultAsync(s => s.LibraryId == libraryId && s.BookEditionId == bookId);

			//return stock != null;

			return false;
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
