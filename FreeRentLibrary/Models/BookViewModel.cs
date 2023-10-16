using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class BookViewModel : Book
    {
        public IEnumerable<SelectListItem> Authors { get; set; }

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();

        [Required(ErrorMessage = "Please select at least one genre.")]
        public List<int> SelectedGenres { get; set; }
    }
}
