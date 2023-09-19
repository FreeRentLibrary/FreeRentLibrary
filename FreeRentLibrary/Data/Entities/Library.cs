using System.Collections.Generic;

namespace FreeRentLibrary.Data.Entities
{
    public class Library
    {
        public int Id { get; set; }

        public List<Book> Books { get; set; }

        public User User { get; set; }
    }
}
