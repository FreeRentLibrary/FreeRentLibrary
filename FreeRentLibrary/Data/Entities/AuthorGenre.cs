using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class AuthorGenre : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? AuthorId { get; set; }

        public Author Author { get; set; }

        public int? GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
