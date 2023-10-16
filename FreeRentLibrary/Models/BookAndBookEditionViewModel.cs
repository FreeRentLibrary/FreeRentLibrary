using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class BookAndBookEditionViewModel
    {
        [Display(Name = "Books")]
        public int BookId { get; set; }
        public IEnumerable<SelectListItem> Books { get; set; }

        [Display(Name = "Title")]
        public string? Name { get; set; }

        public string? Synopsis { get; set; }

        public string? NativeLanguage { get; set; }

        [Display(Name = "Author")]
        public int? AuthorId { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }

        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();

        public List<int>? SelectedGenres { get; set; }

        [Display(Name = "Edition Name")]
        public string? EditionName { get; set; }

        [Display(Name = "Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a book type!")]
        public int BookTypeId { get; set; }

        public IEnumerable<SelectListItem> BookType { get; set; }

        [Display(Name = "Publisher")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Publisher!")]
        public int BookPublisherId { get; set; }

        public IEnumerable<SelectListItem> BookPublisher { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; } = DateTime.MinValue;

        [Display(Name = "Page Count")]
        public int? PageCount { get; set; }

        [Required(ErrorMessage = "You must insert a ISBN code here.")]
        [Display(Name = "ISBN 13")]
        public string ISBN { get; set; }

        [Display(Name = "Age Restriction")]
        [DisplayFormat(DataFormatString = "{0:N}+")]
        public int? MinimumAge { get; set; }

        [Display(Name = "Language")]
        public string? TranslatedLanguage { get; set; }

        public string? Translator { get; set; }

        public bool SameBookName { get; set; }

        public bool CreateNewBook { get; set; }
    }
}
