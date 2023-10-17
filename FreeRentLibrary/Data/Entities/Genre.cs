using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Genre : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }

        public ICollection<AuthorGenre> AuthorGenres { get; set; }

    }
}
