using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FreeRentLibrary.Data
{
    public class LibraryRepository : GenericRepository<Book>, ILibraryRepository
    {
        private readonly DataContext _context;

        public LibraryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetUserBooks()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable GetUserLibrary()
        {
            throw new System.NotImplementedException();
        }

        /*public IQueryable GetUserLibrary()
        {

           return _context.Books.Include(b => b.User);
        }

        public IEnumerable<SelectListItem> GetUserBooks()
        {
           var list = _context.Books.Select(p => new SelectListItem
           {
               Text = p.Title,
               Value = p.Id.ToString()

           }).ToList();
           list.Insert(0, new SelectListItem
           {
               Text = "(Select a product...)",
               Value = "0"
           });
           return list;
        }*/

    }
}
