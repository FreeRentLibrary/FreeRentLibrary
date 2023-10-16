using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;

namespace FreeRentLibrary.Models
{
    public class LibraryViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public BookEdition BookOfTheDay { get; set; }
        public IEnumerable<Book> BooksByGenre { get; set; }
        public IEnumerable<Book> BooksByAuthor { get; set; }

        //Search Functions
        public string SelectedSearchCategory { get; set; }
        public string SearchQuery { get; set; }
        public List<string> SearchCategories { get; set; }
        
    }
}
