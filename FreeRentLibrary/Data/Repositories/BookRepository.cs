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
                .Include(b => b.BookGenres)
                .OrderBy(b => b.Name);
        }

        public async Task AddBookAsync(BookViewModel viewModel)
        {
            var author = await _context.Authors.FindAsync(viewModel.AuthorId);
            if (author == null)
            {
                return;
            }

            var book = new Book
            {
                Name = viewModel.Name,
                Synopsis = viewModel.Synopsis,
                NativeLanguage = viewModel.NativeLanguage,
                AuthorId = viewModel.AuthorId,
                Author = _context.Authors
                    .Where(a => a.Id == viewModel.AuthorId)
                    .FirstOrDefault(),
            };

            var selectedGenres = await _context.Genres
                .Where(g => viewModel.SelectedGenres.Contains(g.Id))
                .ToListAsync();

            var bookGenres = selectedGenres.Select(genre => new BookGenre
            {
                GenreId = genre.Id,
                Genre = genre,
                BookId = book.Id,
                Book = book,
            }).ToList();

            book.BookGenres = bookGenres;

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
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Where(b => b.Id == bookId);

            var book = await bookQuery.FirstOrDefaultAsync();

            if (book != null)
            {
                await _context.Entry(book)
                    .Collection(b => b.BookEditions)
                    .Query()
                    .Include(be => be.BookPublisher)
                    .Include(be => be.BookType)
                    .LoadAsync();
            }

            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Where(b => b.Author.Id == authorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Where(b => b.BookGenres.Any(g => g.Id == genreId))
                .ToListAsync();
        }

        #endregion

        //--

        #region BookEdition

        public async Task AddBookEditionAsync(BookEditionViewModel viewModel)
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
                CoverId = viewModel.CoverId,
                BookId = viewModel.BookId,
                ReleaseDate = viewModel.ReleaseDate,
                PageCount = viewModel.PageCount,
                ISBN = viewModel.ISBN,
                MinimumAge = viewModel.MinimumAge,
                TranslatedLanguage = viewModel.TranslatedLanguage,
                Translator = viewModel.Translator,
                BookPublisherId = viewModel.BookPublisherId,
                BookPublisher = _context.Publishers.Where(p => p.Id == viewModel.BookPublisherId).FirstOrDefault(),

            });
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookEditionAsync(BookEdition bookEdition)
        {
            _context.BookEditions.Update(bookEdition);
            await _context.SaveChangesAsync();
        }

        public async Task<BookEdition> GetBookEditionAsync(int bookEditionId)
        {
            return await _context.BookEditions
                .Include(be => be.Book)
                .ThenInclude(b => b.BookGenres)
                .Include(be => be.Book)
                .ThenInclude(b => b.Author)
                .Include(be => be.BookPublisher)
                .Include(be => be.BookType)
                .Where(be => be.Id == bookEditionId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BookEditionExistsAsync(int bookEditionId)
        {
            return await _context.BookEditions.AnyAsync(be => be.Id == bookEditionId);
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

        public IEnumerable<SelectListItem> GetComboBooks(int bookId)
        {
            var book = _context.Books.Find(bookId);
            var list = new List<SelectListItem>();
            if (book != null)
            {
                list = _context.Books.Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

            }
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

        public IEnumerable<SelectListItem> GetComboBookTypes(int typeId)
        {
            var type = _context.BookTypes.Find(typeId);
            var list = new List<SelectListItem>();
            if (type != null)
            {
                list = _context.BookTypes.Select(bt => new SelectListItem
                {
                    Text = bt.Name,
                    Value = bt.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

            }
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

        public IEnumerable<SelectListItem> GetComboBookPublishers(int publisherId)
        {
            var publisher = _context.Publishers.Find(publisherId);
            var list = new List<SelectListItem>();
            if (publisher != null)
            {
                list = _context.Publishers.Select(bp => new SelectListItem
                {
                    Text = bp.Name,
                    Value = bp.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

            }
            return list;
        }

        #endregion
    }
}
