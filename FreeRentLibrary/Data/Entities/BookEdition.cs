using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class BookEdition : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        //ISBN code of a book
        [Required(ErrorMessage = "You must insert a ISBN code here.")]
        [Display(Name = "ISBN 13")]
        public string ISBN { get; set; }

        [Display(Name = "Cover")]
        public Guid CoverId { get; set; }

        [Display(Name = "Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a book type!")]
        public int BookTypeId { get; set; }

        public BookTypes BookType { get; set; }

        //Usualy is the Book name, unless it changes something in translation like (Harry Potter e a Pedra Filosofal)
        [Display(Name = "Edition Name")]
        public string? EditionName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; } = DateTime.MinValue;

        //Number of pages in the edition
        [Display(Name = "Page Count")]
        public int? PageCount { get; set; }

        //Recomended minimum age to read
        [Display(Name = "Age Restriction")]
        [DisplayFormat(DataFormatString = "{0:N}+")]
        public int? MinimumAge { get; set; }

        //In case the edition is a translated one
        [Display(Name = "Language")]
        public string? TranslatedLanguage { get; set; }

        public string? Translator { get; set; }

        [Display(Name = "Publisher")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Publisher!")]
        public int BookPublisherId { get; set; }

        public BookPublisher BookPublisher { get; set; }

        //Shows the Libraries that have the Book and the respective Stock
        public ICollection<LibraryStock> LibraryStocks { get; set; }

        public string ImageFullPath => CoverId == Guid.Empty
            ? $"https://frlcontainer.blob.core.windows.net/default/noimage.png"
            : $"https://frlcontainer.blob.core.windows.net/covers/{CoverId}";
    }
}
