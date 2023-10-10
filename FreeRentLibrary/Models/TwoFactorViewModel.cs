using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FreeRentLibrary.Models
{
    public class TwoFactorViewModel
    {
        [Required]
        [Display(Name = "Two-Factor Authentication Code")]
        public string TwoFactorCode { get; set; }
    }
}
