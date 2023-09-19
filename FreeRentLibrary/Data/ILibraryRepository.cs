using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FreeRentLibrary.Data
{
    public interface ILibraryRepository : IGenericRepository<Book>
    {
        public IQueryable GetUserLibrary();

        IEnumerable<SelectListItem> GetUserBooks();
    }
}
