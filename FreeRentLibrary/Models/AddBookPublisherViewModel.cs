using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class AddBookPublisherViewModel
    {
        [Required(ErrorMessage = "Please write a name.")]
        public string Name { get; set; }


        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
