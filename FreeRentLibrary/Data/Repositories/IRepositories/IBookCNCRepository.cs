using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IBookCNCRepository : IGenericRepository<Book>
    {
        IQueryable GetBooksWithUsers();
        Task<IEnumerable<Book>> GetBooksByNameAsync(string name);
        Task<Book> GetBookWithAuthorAsync(string author);
        IEnumerable<SelectListItem> GetComboBooks();
    }
}
