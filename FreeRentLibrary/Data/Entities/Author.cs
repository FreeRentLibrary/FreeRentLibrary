using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Author : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //public string PenName { get; set; }

        public ICollection<Book> Books { get; set; }

        public ICollection<Genre> Genres { get; set; }
    }
}
