using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class LibraryViewModel : Library
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<BookEdition> BookEditions { get; set; }
        public BookEdition BookOfTheDay { get; set; }
        public IEnumerable<Book> BooksByGenre { get; set; }
        public IEnumerable<Book> BooksByAuthor { get; set; }


        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        //Search Functions
        public string SelectedSearchCategory { get; set; }
        public string SearchQuery { get; set; }
        public List<string> SearchCategories { get; set; }
        
    }
}
