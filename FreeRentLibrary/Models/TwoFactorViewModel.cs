using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FreeRentLibrary.Models
{
    public class TwoFactorViewModel
    {

        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "Two-Factor Authentication Code")]
        public string TwoFactorCode { get; set; }
    }
}
