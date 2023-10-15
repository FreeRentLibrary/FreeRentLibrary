using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories
{
    public class BookRepository:GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context):base(context)
        {
            _context = context;
        }

        public IQueryable GetBooksWithAuthorsAndGenres()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .OrderBy(b => b.Name);
        }

        public async Task AddBookAsync(AddBookViewModel viewModel)
        {
            var book = new Book
            {
                Name = viewModel.Name,
                Synopsis = viewModel.Synopsis,
                NativeLanguage = viewModel.NativeLanguage,
                AuthorId = viewModel.AuthorId,
                Author = _context.Authors
                    .Where(a => a.Id == viewModel.AuthorId)
                    .FirstOrDefault(),
                Genres = _context.Genres
                    .Where(g => viewModel.SelectedGenres
                    .Contains(g.Id))
                    .ToList(),
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public bool CheckIfBookExists(AddBookViewModel viewModel)
        {
            bool bookCheck = _context.Books
                .Any(b => b.Name == viewModel.Name
                && b.Author.Id == viewModel.AuthorId);

            return bookCheck;
        }

        public async Task<Book> GetBookWithNameAsync(string bookName)
        {
            return await _context.Books
                .Where(b => b.Name == bookName)
                .FirstOrDefaultAsync();
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
    }
}
