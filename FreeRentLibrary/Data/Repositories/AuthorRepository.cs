using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
                .Include(a => a.Genres)
                .Include(a => a.Books)
                .OrderBy(a => a.Name);
        }

        public async Task AddAuthorWithGenresAsync(AddAuthorViewModel viewModel)
        {
            var author = new Author
            {
                Name = viewModel.Name,
                Genres = _context.Genres
                    .Where(g => viewModel.SelectedGenres
                    .Contains(g.Id))
                    .ToList(),
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
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
    }
}
