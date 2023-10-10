using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeRentLibrary.Data
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        IQueryable GetBooksWithUsers();
        Task<IEnumerable<Book>> GetBooksByNameAsync(string name);
        Task<Book> GetBookWithAuthorAsync(string author);
        IEnumerable<SelectListItem> GetComboBooks();
    }
}
