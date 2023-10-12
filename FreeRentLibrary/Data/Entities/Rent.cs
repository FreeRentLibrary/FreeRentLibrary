using System;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Rent : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rental Date")]
        public DateTime? RentDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        public int? LibraryId { get; set; }

        public Library Library { get; set; }

        public string? UserId { get; set; }

        public User User { get; set; }

        public int? BookEditionId { get; set; }

        public BookEdition BookEdition { get; set; }
    }
}
