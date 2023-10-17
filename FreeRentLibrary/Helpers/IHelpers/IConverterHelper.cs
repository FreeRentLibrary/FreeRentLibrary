using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using System;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IConverterHelper
    {
        BookViewModel ToBookViewModel(BookAndBookEditionViewModel bbViewModel);

        BookEdition ToBookEdition(BookEditionViewModel viewModel);

        BookEditionViewModel ToBookEditionViewModel(BookEdition bookEdition);

        BookEditionViewModel ToBookEditionViewModel(BookAndBookEditionViewModel bbViewModel, Guid imageId);
    }
}
