using FreeRentLibrary.Controllers;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories
{
	public class LibraryRepository : GenericRepository<Library>, ILibraryRepository
	{
		private readonly DataContext _context;
		private readonly RentRepository _rentRepository;
		private readonly ReserveRepository _reserveRepository;

		public LibraryRepository(DataContext context): base(context)
		{
			_context = context;
		}

        public IQueryable GetLibrariesWithCity()
        {
			return _context.Libraries
				.Include(l => l.City)
				.OrderBy(l => l.Id);
        }

        public async Task AddBookToStock(int bookEditionId, int libraryId, int quantity)
        {
            var bookEdition = await _context.BookEditions.FirstOrDefaultAsync(be => be.Id == bookEditionId);

            var library = await _context.Libraries.FirstOrDefaultAsync(l => l.Id == libraryId);

            var stock = new LibraryStock
            {
                BookEditionId = bookEdition.Id,
                BookEdition = bookEdition,
                LibraryId = library.Id,
                Library = library,
                Quantity = quantity
            };

            _context.LibraryStocks.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<Library> GetLibraryWithAllInfo(int libraryId)
        {
            var libraryQuery = _context.Libraries
                .Include(l => l.City)
                .Include(l => l.LibraryStocks)
                .ThenInclude(ls => ls.BookEdition)
                .ThenInclude(be => be.Book)
                .ThenInclude(b => b.Author)
                .Include(l => l.LibraryStocks)
                .ThenInclude(ls => ls.BookEdition)
                .ThenInclude(be => be.BookType)
                .Include(l => l.LibraryStocks)
                .ThenInclude(ls => ls.BookEdition)
                .ThenInclude(be => be.BookPublisher)
                .Include(l => l.Rentals)
                .ThenInclude(r => r.BookEdition)
                .Include(l => l.Reservations)
                .ThenInclude(r => r.BookEdition)
                .Where(l => l.Id == libraryId);

            var library = await libraryQuery.FirstOrDefaultAsync();

            return library;
        }

        public async Task<bool> CheckStockAsync(int libraryId, int bookId)
		{
			//var stock = await _context.LibraryStocks
			//	.FirstOrDefaultAsync(s => s.LibraryId == libraryId && s.BookEditionId == bookId);

			//return stock != null;

			return false;
		}

        public IEnumerable<SelectListItem> GetComboLibrary()
        {
            var list = _context.Libraries.Select(bp => new SelectListItem
            {
                Text = bp.Name,
                Value = bp.Id.ToString()
            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select a Library...",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboLibrary(int libraryId)
        {
            var publisher = _context.Libraries.Find(libraryId);
            var list = new List<SelectListItem>();
            if (publisher != null)
            {
                list = _context.Libraries.Select(bp => new SelectListItem
                {
                    Text = bp.Name,
                    Value = bp.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

            }
            return list;
        }

    }
}
