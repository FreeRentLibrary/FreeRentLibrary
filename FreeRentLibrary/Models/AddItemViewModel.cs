using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Book")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a book.")]
        public int BookId { get; set; }


        [Range(0.0001, double.MaxValue, ErrorMessage = "You must select a quantity.")]
        public double Quantity { get; set; }

        public IEnumerable<SelectListItem> Books { get; set; }
    }
}
