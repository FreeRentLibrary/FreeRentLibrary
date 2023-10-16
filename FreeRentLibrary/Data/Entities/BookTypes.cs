using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class BookTypes:IEntity
    {
        [Key]
        public int Id { get; set; }

        //ex. Hardcover/Paperback
        public string Name { get; set; }

        public ICollection<BookEdition> BookEditions { get; set; }
    }
}
