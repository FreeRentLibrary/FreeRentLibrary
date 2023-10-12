using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FreeRentLibrary.Data.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksByNameAsync(string name)
        {
            return await _context.Books.Where(b => b.Name == name).ToListAsync();
        }

        //TODO: In progress
        public IQueryable GetBooksWithUsers()
        {
            //return _context.Books.Include(b => b.User).OrderBy(b => b.Name);
            //To change after
            return _context.Books;
        }

        public Task<Book> GetBookWithAuthorAsync(string author)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetComboBooks()
        {
            var list = _context.Books.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select a book",
                Value = "0"
            });
            return list;
        }
    }
}
