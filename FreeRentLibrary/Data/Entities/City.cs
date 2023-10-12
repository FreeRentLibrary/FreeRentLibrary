using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class City : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(255, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
