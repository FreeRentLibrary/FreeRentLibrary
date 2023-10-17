using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories
{
    public class BookPublisherRepository:GenericRepository<BookPublisher>, IBookPublisherRepository
    {
        private readonly DataContext _context;

        public BookPublisherRepository(DataContext context):base(context)
        {
            _context = context;
        }

        public IQueryable GetPublishersWithCountry()
        {
            return _context.Publishers
                .Include(g => g.Country)
                .OrderBy(g => g.Name);
        }

        public async Task<IEnumerable<BookPublisher>> SearchBookPublisherAsync(string query)
        {
            return await _context.Publishers
                .Include(p => p.Editions)
                .ThenInclude(be => be.Book)
                .ThenInclude(b => b.Author)
                .Where(p => p.Name.Contains(query))
                .ToListAsync();
        }

        public async Task<BookPublisher> AddBookPublisherWithCountry(BookPublisherViewModel viewModel)
        {
            var bookPublisher = new BookPublisher
            {
                Name = viewModel.Name,
                CountryId = viewModel.CountryId,
                Country = _context.Countries
                    .Where(c => c.Id == viewModel.CountryId)
                    .FirstOrDefault(),
            };

            _context.Publishers.Add(bookPublisher);
            await _context.SaveChangesAsync();
            return bookPublisher;
        }

        public async Task<BookPublisher> GetPublisherWithNameAsync(string publisherName)
        {
            return await _context.Publishers
                .Where(p => p.Name == publisherName)
                .FirstOrDefaultAsync();
        }

        public async Task<BookPublisher> GetPublisherWithBooksAndCountry(int publisherId)
        {
            return await _context.Publishers
                .Include(p => p.Editions)
                .Include(p => p.Country)
                .Where(p => p.Id == publisherId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BookPublisher>> GetPublishersByCountryAsync(int countryId)
        {
            return await _context.Publishers
                .Include(g => g.Country)
                .Where(p => p.Country.Id == countryId)
                .ToListAsync();
        }
    }
}
