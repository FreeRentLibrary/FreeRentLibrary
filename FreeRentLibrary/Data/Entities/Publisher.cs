using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Publisher : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Country Country { get; set; }

        //TODO: URL connection to website
        public string Url { get; set; }

        //Change to ICollection in case of failure
        public IEnumerable<BookEdition> Editions { get; set; }
    }
}
