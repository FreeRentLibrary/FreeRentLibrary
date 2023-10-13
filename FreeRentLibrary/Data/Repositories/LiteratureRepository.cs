using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories
{
    public class LiteratureRepository : GenericRepository<Book>, ILiteratureRepository
    {
        private readonly DataContext _context;

        public LiteratureRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        #region Author Related

        public IQueryable GetAuthorsWithGenresAndBooks()
        {
            return _context.Authors
                .Include(a => a.Genres)
                .Include(a => a.Books)
                .OrderBy(a => a.Name);
        }
        
        public async Task<Author> GetAuthorWithGenresAndBooks(int authorId)
        {
            return await _context.Authors
                .Include(a => a.Genres)
                .Include(a => a.Books)
                .Where(a => a.Id == authorId)
                .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<Author>> GetAuthorsByGenreAsync(int genreId)
        {
            return await _context.Authors
                .Include(a => a.Genres)
                .Where(a => a.Genres.Any(g => g.Id == genreId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByGenreListAsync(IEnumerable<int> genreIdList)
        {
            var authors = _context.Authors
                .Where(a => a.Genres.Any(g => genreIdList.Contains(g.Id)))
                .GroupBy(a => a.Id)
                .Where(group => group.Count() == genreIdList.Count())
                .Select(group => group.FirstOrDefault());

            return await authors.ToListAsync();
        }

        #endregion

        //--

        #region Book Related

        public IQueryable GetBooksWithAuthorsAndGenres()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .OrderBy(b => b.Name);
        }

        public async Task<Book> GetBookWithAllDataAsync(int bookId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .Include(b => b.BookEditions)
                .Where(b => b.Id == bookId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .Where(b => b.Author.Id == authorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .Where(b => b.Genres.Any(g => g.Id == genreId))
                .ToListAsync();
        }

        public async Task<BookEdition> GetBookEditionAsync(int bookEditionId)
        {
            return await _context.BookEditions
                .Include(be => be.Book)
                .Include(be => be.Publisher)
                .Where(be => be.Id == bookEditionId)
                .FirstOrDefaultAsync();
        }

        #endregion

        //--

        #region Genre Related

        public IQueryable GetGenresWithAuthorsAndBooks()
        {
            return _context.Genres
                .Include(g => g.Authors)
                .Include(g => g.Books)
                .OrderBy(g => g.Name);
        }

        public Task<Genre> GetGenreWithAuthorsAndBooks(int genreId)
        {
            return _context.Genres
                .Include(g => g.Authors)
                .Include(g => g.Books)
                .Where(g => g.Id == genreId)
                .FirstOrDefaultAsync();
        }

        #endregion

        //--

        #region Publisher Related

        public IQueryable GetPublishersWithCountry()
        {
            return _context.Publishers
                .Include(g => g.Country)
                .OrderBy(g => g.Name);
        }

        public async Task<Publisher> GetPublisherWithBooksAndCountry(int publisherId)
        {
            return await _context.Publishers
                .Include(p => p.Editions)
                .Include(p => p.Country)
                .Where(p => p.Id == publisherId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Publisher>> GetPublishersByCountryAsync(int countryId)
        {
            return await _context.Publishers
                .Include(g => g.Country)
                .Where(p => p.Country.Id == countryId)
                .ToListAsync();
        }

        #endregion

    }
}
