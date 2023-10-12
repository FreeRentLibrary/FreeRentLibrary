using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class LibraryStock : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? LibraryId { get; set; }

        public Library Library { get; set; }

        public int? BookEditionId { get; set; }

        public BookEdition BookEdition { get; set; }

        public int Quantity { get; set; }
    }
}
