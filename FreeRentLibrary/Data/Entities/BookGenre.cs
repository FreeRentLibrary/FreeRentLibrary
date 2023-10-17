using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class BookGenre:IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? BookId { get; set; }

        public Book Book { get; set; }

        public int? GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
