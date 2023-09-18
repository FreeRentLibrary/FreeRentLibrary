using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using System;
using System.IO;

namespace FreeRentLibrary.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Book ToBook(BookViewModel model, Guid imageId, bool isNew)
        {
            return new Book
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                IsAvailable = model.IsAvailable,
                RentDate = model.RentDate,
                DueDate = model.DueDate,
                Title = model.Title,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        public BookViewModel ToBookViewModel(Book product)
        {
            return new BookViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                RentDate = product.RentDate,
                DueDate = product.DueDate,
                ImageId = product.ImageId,
                Price = product.Price,
                Title = product.Title,
                Stock = product.Stock,
                User = product.User
            };
        }
    }
}
