using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using System;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IConverterHelper
    {
        BookEditionViewModel ToBookEditionViewModel(BookAndBookEditionViewModel bbViewModel, Guid imageId);

        Task ReserveToRentAsync(Reservation reservation);

        BookViewModel ToBookViewModel(BookAndBookEditionViewModel bbViewModel);
    }
}
