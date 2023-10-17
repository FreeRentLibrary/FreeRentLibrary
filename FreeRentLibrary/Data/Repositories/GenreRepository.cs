using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories
{
    public class GenreRepository:GenericRepository<Genre>, IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context):base(context)
        {
            _context = context;
        }

        public IQueryable GetGenresWithAuthorsAndBooks()
        {
            return _context.Genres
                .Include(g => g.AuthorGenres)
                .Include(g => g.BookGenres)
                .OrderBy(g => g.Name);
        }

        public Task<Genre> GetGenreWithAuthorsAndBooks(int genreId)
        {
            return _context.Genres
                .Include(g => g.AuthorGenres)
                .Include(g => g.BookGenres)
                .Where(g => g.Id == genreId)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Genre> GetGenres(List<int> genresIdList)
        {
            return _context.Genres
                .Where(g => genresIdList
                .Contains(g.Id))
                .ToList();
        }
    }
}
