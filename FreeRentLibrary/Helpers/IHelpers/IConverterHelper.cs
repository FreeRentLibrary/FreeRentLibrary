using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using System;

namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IConverterHelper
    {
        Book ToBook(BookViewModel model, Guid imageId, bool isNew);
        BookViewModel ToBookViewModel(Book product);
    }
}
