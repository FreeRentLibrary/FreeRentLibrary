using System;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class Book : IEntity
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "Caracteres demais")]
        public string Title { get; set; }

        //A resolver...
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }


        [Display(Name = "Image")]
        public Guid ImageId { get; set; }


        [Display(Name = "Rent Date")]
        public DateTime? RentDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }


        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        //Remover
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }


        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"/images/noimage.png"
            : $"https://supershoptpsirs.blob.core.windows.net/products/{ImageId}";

    }
}
