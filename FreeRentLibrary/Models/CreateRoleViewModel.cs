using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
