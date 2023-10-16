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

        public int? BookTypeId { get; set; }

        public BookTypes BookType { get; set; }

        //Usualy is the Book name, unless it changes something in translation like (Harry Potter e a Pedra Filosofal)
        public string EditionName { get; set; }

        public int? BookId { get; set; }

        public Book Book { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }

        //Number of pages in the edition
        public int? Pages { get; set; }

        //ISBN code of a book
        public string ISBN { get; set; }

        //Recomended minimum age to read
        public int? MinimumAge { get; set; }

        //In case the edition is a translated one
        public string? TranslatedLanguage { get; set; }

        public string? Translator { get; set; }


        public int? PublisherId { get; set; }

        public BookPublisher Publisher { get; set; }

        //Shows the Libraries that have the Book and the respective Stock
        public ICollection<LibraryStock> LibraryStocks { get; set; }
    }
}
