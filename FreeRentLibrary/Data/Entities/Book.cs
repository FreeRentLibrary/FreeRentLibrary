using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Book : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Title { get; set; }
        public string Category { get; set; }
        public uint TotalPages { get; set; }
        public uint CurrentPage { get; set; }
        [Required]
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        
        [Required]
        public string Publisher { get; set; }

    }
}
