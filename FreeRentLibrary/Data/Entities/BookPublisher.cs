using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class BookPublisher : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        //TODO: URL connection to website
        //public string Url { get; set; }

        //Change to ICollection in case of failure
        public ICollection<BookEdition> Editions { get; set; }
    }
}
