using System.ComponentModel.DataAnnotations;
using System;

namespace FreeRentLibrary.Data.Entities
{
    public class Reservation : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? BookId { get; set; }

        [Display(Name = "Reservation Date")]
        public DateTime? ReservationDate { get; set; }

        // The Day the Reservation Ended - Reasons: Cancelled, Rented (PS: DueDate was taken)
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
