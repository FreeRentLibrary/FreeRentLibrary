using System.Collections.Generic;
using System.Linq;
using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeRentLibrary.Data
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboBooks();
    }
}
