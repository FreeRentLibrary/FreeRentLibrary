using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FreeRentLibrary.Models
{
    public class LibraryStockViewModel : LibraryStock
    {
        public IEnumerable<SelectListItem> BookEditions { get; set; }

        public IEnumerable<SelectListItem> Libraries { get; set; }
    }
}
