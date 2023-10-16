using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        #region Book

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

        public async Task DeleteBookAsync(int bookId)
        {
            var bookEditions = _context.BookEditions
                .Where(be => be.BookId == bookId);
            _context.BookEditions.RemoveRange(bookEditions);

            var book = await _context.Books.FindAsync(bookId);
            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
        }

        public bool CheckIfBookExists(string bookName, int authorId)
        {
            bool bookCheck = _context.Books
                .Any(b => b.Name == bookName
                && b.Author.Id == authorId);

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
            var bookQuery = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .Where(b => b.Id == bookId);

            var book = await bookQuery.FirstOrDefaultAsync();

            if (book != null)
            {
                await _context.Entry(book)
                    .Collection(b => b.BookEditions)
                    .Query()
                    .Include(be => be.Publisher)
                    .Include(be => be.BookType)
                    .LoadAsync();
            }

            return book;
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

        #endregion

        //--

        #region BookEdition

        public async Task AddBookEditionAsync(AddBookEditionViewModel viewModel)
        {
            var book = await GetBookWithAllDataAsync(viewModel.BookId);
            if (book == null)
            {
                return;
            }
            if (viewModel.EditionName == null)
            {
                viewModel.SameBookName = true;
            }
            book.BookEditions.Add(new BookEdition
            {
                BookTypeId = viewModel.BookTypeId,
                BookType = _context.BookTypes.Where(bt => bt.Id == viewModel.BookTypeId).FirstOrDefault(),
                EditionName = viewModel.SameBookName ? book.Name : viewModel.EditionName,
                Book = book,
                BookId = viewModel.BookId,
                ReleaseDate = viewModel.ReleaseDate,
                Pages = viewModel.PageCount,
                ISBN = viewModel.ISBN,
                MinimumAge = viewModel.MinimumAge,
                TranslatedLanguage = viewModel.TranslatedLanguage,
                Translator = viewModel.Translator,
                PublisherId = viewModel.BookPublisherId,
                Publisher = _context.Publishers.Where(p => p.Id == viewModel.BookPublisherId).FirstOrDefault(),

            });
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<BookEdition> GetBookEditionAsync(int bookEditionId)
        {
            return await _context.BookEditions
                .Include(be => be.Book)
                .ThenInclude(b => b.Genres)
                .Include(be => be.Book)
                .ThenInclude(b => b.Author)
                .Include(be => be.Publisher)
                .Include(be => be.BookType)
                .Where(be => be.Id == bookEditionId)
                .FirstOrDefaultAsync();
        }

        #endregion

        //--

        #region Combo

        public IEnumerable<SelectListItem> GetComboBooks()
        {
            var list = _context.Books.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select a Book...",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboBookTypes()
        {
            var list = _context.BookTypes.Select(bt => new SelectListItem
            {
                Text = bt.Name,
                Value = bt.Id.ToString()
            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select a Book Type...",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboBookPublishers()
        {
            var list = _context.Publishers.Select(bp => new SelectListItem
            {
                Text = bp.Name,
                Value = bp.Id.ToString()
            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select a Publisher...",
                Value = "0"
            });
            return list;
        }

        #endregion
    }
}
