using FreeRentLibrary.Data.Entities;
using System.Collections;
using System.Collections.Generic;

namespace FreeRentLibrary.Models
{
    public class SearchResultsViewModel
    {
        public IEnumerable<BookEdition> BookEditionResults { get; set; } = new List<BookEdition>();

        public IEnumerable<Author> AuthorResults { get; set; } = new List<Author>();

        public IEnumerable<BookPublisher> PublisherResults { get; set; } = new List<BookPublisher>();
    }
}
