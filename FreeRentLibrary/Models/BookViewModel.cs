using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Models
{
    public class BookViewModel : Book
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

    }
}
