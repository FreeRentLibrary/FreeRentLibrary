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

        //Edition Type (ex. Hardback Children's Edition)
        public string EditionType { get; set; }

        //Usualy is the Book name, unless it changes something in translation like (Harry Potter e a Pedra Filosofal)
        public string EditionName { get; set; }

        public int? BookId { get; set; }

        public Book Book { get; set; }

        public DateTime ReleaseDate { get; set; }

        //Number of pages
        public int Pages { get; set; }

        //Language that the book was written
        public string NativeLanguage { get; set; }

        //ISBN code of a book
        public string ISBN { get; set; }

        //Recomended minimum age to read
        public int MinimumAge { get; set; }

        //In case the edition is a translated one
        public string? TranslatedLanguage { get; set; }

        public string? Translator { get; set; }


        public int? PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        //Shows the Libraries that have the Book and the respective Stock
        public IEnumerable<LibraryStock> LibraryStocks { get; set; }
    }
}
