using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Author : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please write a name.")]
        public string Name { get; set; }

        [Display(Name = "Photo")]
        public Guid AuthorPhotoId { get; set; }

        //public string PenName { get; set; }

        [Display(Name = "Books")]
        public ICollection<Book> Books { get; set; }

        [Display(Name = "Genres")]
        public ICollection<AuthorGenre> AuthorGenres { get; set; }

        public string ImageFullPath => AuthorPhotoId == Guid.Empty
            ? $"https://frlcontainer.blob.core.windows.net/default/noimage.png"
            : $"https://frlcontainer.blob.core.windows.net/authors/{AuthorPhotoId}";
    }
}
