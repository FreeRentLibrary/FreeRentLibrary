using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
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
    }
}
