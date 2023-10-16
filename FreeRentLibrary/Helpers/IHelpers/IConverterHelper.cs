using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using System;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IConverterHelper
    {
        Book ToBook(BookViewModel model, Guid imageId, bool isNew);
        BookViewModel ToBookViewModel(Book product);

        BookEditionViewModel ToBookEditionViewModel(BookAndBookEditionViewModel bbViewModel, Guid imageId);

        AddBookViewModel ToBookViewModel(BookAndBookEditionViewModel bbViewModel);

        Task ReserveToRentAsync(Reservation reservation);
        BookViewModel ToBookViewModel(BookAndBookEditionViewModel bbViewModel);
    }
}
