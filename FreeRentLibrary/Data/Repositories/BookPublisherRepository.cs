using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
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
