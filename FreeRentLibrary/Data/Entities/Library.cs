using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Library
    {
        [Key]
        public int Id { get; set; }

        //TODO: Create class Stock/LibraryStock for stock of books

        //TODO: Other data like address, name, city, etc...

        //TODO: Connect to classes of Rent and Reservation
    }
}
