using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class AddBookViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Name { get; set; }

        public string Synopsis { get; set; }

        public string NativeLanguage { get; set; }

        [Display(Name = "Author")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an Author")]
        public int AuthorId { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();

        [Required(ErrorMessage = "Please select at least one genre.")]
        public List<int> SelectedGenres { get; set; }
    }
}
