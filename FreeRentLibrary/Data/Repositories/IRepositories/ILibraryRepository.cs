using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface ILibraryRepository : IGenericRepository<Book>
    {
        IQueryable GetUserLibrary();

        IEnumerable<SelectListItem> GetUserBooks();

        Task<bool> CheckStockAsync(int libraryId, int bookId);
	}
}
