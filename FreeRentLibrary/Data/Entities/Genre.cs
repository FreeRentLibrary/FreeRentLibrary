using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Genre : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
