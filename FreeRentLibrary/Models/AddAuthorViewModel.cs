using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Models
{
    public class AddAuthorViewModel
    {
        [Required(ErrorMessage = "Please write a name.")]
        public string Name { get; set; }

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();

        [Required(ErrorMessage = "Please select at least one genre.")]
        public List<int> SelectedGenres { get; set; }
    }
}
