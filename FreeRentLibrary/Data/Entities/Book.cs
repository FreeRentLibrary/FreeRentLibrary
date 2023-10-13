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
              
        /*[Display(Name = "Image")]
        public Guid ImageId { get; set; }*/

        //TODO: Remove proprieties
        /*[Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }*/

        public string Synopsis { get; set; }

        //Language that the book was written
        public string NativeLanguage { get; set; }

        //Change to ICollection in case of failure
        public ICollection<BookEdition> BookEditions { get; set; }

        public ICollection<Genre> Genres { get; set; }

        public int? AuthorId { get; set; }

        public Author Author { get; set; }


        /*public string ImageFullPath => ImageId == Guid.Empty
            ? $"/images/noimage.png"
            : $"https://supershopsi.blob.core.windows.net/books/{ImageId}";*/

    }
}
