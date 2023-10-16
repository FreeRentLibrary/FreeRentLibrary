using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Book : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Name { get; set; }

        public string Synopsis { get; set; }

        //Language that the book was written
        [Display(Name = "Language")]
        public string NativeLanguage { get; set; }

        //Change to ICollection in case of failure
        public ICollection<BookEdition> BookEditions { get; set; }

        public ICollection<Genre> Genres { get; set; }

        [Display(Name = "Author")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an Author")]
        public int AuthorId { get; set; }

        public Author Author { get; set; }
    }
}
