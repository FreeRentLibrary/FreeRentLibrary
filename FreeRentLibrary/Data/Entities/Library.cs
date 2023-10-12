﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Library : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //Shows the Books of this Library with the respective stocks
        public IEnumerable<LibraryStock> LibraryStocks { get; set; }

        //Rentals made to this library
        public IEnumerable<Rent> Rentals { get; set; }
        
        //Reservations made to this library
        public IEnumerable<Reservation> Reservations { get; set; }

        //TODO: Other data like address, name, city, etc...
        public string Address { get; set; }

        public City City { get; set; }

        
    }
}