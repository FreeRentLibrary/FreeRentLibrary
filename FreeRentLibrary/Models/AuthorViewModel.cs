using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Models
{
    public class AuthorViewModel : Author
    {
        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();

        [Required(ErrorMessage = "Please select at least one genre.")]
        public List<int> SelectedGenres { get; set; }

        [Display(Name = "Author Photo")]
        public IFormFile ImageFile { get; set; }
    }
}
