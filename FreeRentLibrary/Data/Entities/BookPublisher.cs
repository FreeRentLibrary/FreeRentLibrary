using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class BookPublisher : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please write a name.")]
        public string Name { get; set; }

        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        //Change to ICollection in case of failure
        public ICollection<BookEdition> Editions { get; set; }
    }
}
