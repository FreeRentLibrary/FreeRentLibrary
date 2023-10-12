using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Models;
using System;
using System.IO;

namespace FreeRentLibrary.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        //TODO: Make major changes
        public Book ToBook(BookViewModel model, Guid imageId, bool isNew)
        {
            return new Book
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                IsAvailable = model.IsAvailable,
            };
        }

        public BookViewModel ToBookViewModel(Book product)
        {
            return new BookViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                ImageId = product.ImageId,
            };
        }
    }
}
