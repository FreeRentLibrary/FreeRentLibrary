using System;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Edition : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int? BookId { get; set; }

        public Book Book { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int? PublisherId { get; set; }

        public Publisher Publisher { get; set; }
    }
}
