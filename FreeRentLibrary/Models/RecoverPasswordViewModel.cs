using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
