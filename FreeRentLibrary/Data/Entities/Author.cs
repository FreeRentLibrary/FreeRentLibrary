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

        public string PenName { get; set; }

        public IEnumerable<Book> Books { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
    }
}
