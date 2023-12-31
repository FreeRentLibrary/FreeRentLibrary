﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreeRentLibrary.Data.Entities
{
    public class Country : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        [MaxLength(255, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }

        [Display(Name = "Number of cities")]
        public int NumberCities => Cities == null ? 0 : Cities.Count();

        public ICollection<BookPublisher> Publishers { get; set; }
    }
}
