using System.ComponentModel.DataAnnotations;
using System;

namespace FreeRentLibrary.Data.Entities
{
    public class Reserve : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? BookId { get; set; }

        [Display(Name = "Reserve Date")]
        public DateTime? ReserveDate { get; set; }

        // The Day the Reserve Ended - Reasons: Cancelled, Rented (PS: DueDate was taken)
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
