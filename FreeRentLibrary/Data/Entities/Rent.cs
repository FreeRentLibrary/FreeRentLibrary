using System;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Rent : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? BookId { get; set; }

        [Display(Name = "Rental Date")]
        public DateTime? RentDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
