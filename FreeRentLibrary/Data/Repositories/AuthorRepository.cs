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
    public class AuthorRepository:GenericRepository<Author>, IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context):base(context)
        {
            _context = context;
        }

        public IQueryable GetAuthorsWithGenresAndBooks()
        {
            return _context.Authors
                .Include(a => a.AuthorGenres)
                .ThenInclude(ag => ag.Genre)
                .Include(a => a.Books)
                .OrderBy(a => a.Name);
        }

        public async Task<IEnumerable<Author>> SearchAuthorAsync(string query)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .ThenInclude(b => b.BookEditions)
                .ThenInclude(be => be.BookPublisher)
                .Where(a => a.Name.Contains(query))
                .ToListAsync();
        }

        public async Task AddAuthorWithGenresAsync(AuthorViewModel viewModel)
        {
            var author = new Author
            {
                Name = viewModel.Name,
                AuthorPhotoId = viewModel.AuthorPhotoId,
            };

            var selectedGenres = await _context.Genres
                .Where(g => viewModel.SelectedGenres.Contains(g.Id))
                .ToListAsync();

            var authorGenres = selectedGenres.Select(genre => new AuthorGenre
            {
                Author = author,
                Genre = genre
            }).ToList();

            author.AuthorGenres = authorGenres;

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAuthorInfoAsync(int authorId)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .ThenInclude(b => b.BookEditions)
                .FirstOrDefaultAsync(a => a.Id == authorId);

            if (author != null)
            {
                var bookEditionsToRemove = author.Books.SelectMany(a => a.BookEditions).ToList();
                _context.BookEditions.RemoveRange(bookEditionsToRemove);

                var booksToRemove = author.Books.ToList();
                _context.Books.RemoveRange(booksToRemove);

                _context.Authors.Remove(author);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Author> GetAuthorWithGenresAndBooks(int authorId)
        {
            return await _context.Authors
                .Include(a => a.AuthorGenres)
                .Include(a => a.Books)
                .ThenInclude(b => b.BookEditions)
                .Where(a => a.Id == authorId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByGenreAsync(int genreId)
        {
            return await _context.Authors
                .Include(a => a.AuthorGenres)
                .Where(a => a.AuthorGenres.Any(g => g.Id == genreId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByGenreListAsync(IEnumerable<int> genreIdList)
        {
            var authors = _context.Authors
                .Where(a => a.AuthorGenres.Any(g => genreIdList.Contains(g.Id)))
                .GroupBy(a => a.Id)
                .Where(group => group.Count() == genreIdList.Count())
                .Select(group => group.FirstOrDefault());

            return await authors.ToListAsync();
        }

        public IEnumerable<SelectListItem> GetComboAuthors()
        {
            var list = _context.Authors.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select an Author...",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboAuthors(int authorId)
        {
            var author = _context.Authors.Find(authorId);
            var list = new List<SelectListItem>();
            if (author != null)
            {
                list = _context.Authors.Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

            }
            return list;
        }
    }
}
