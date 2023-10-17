using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IConverterHelper
    {

        Book ToBook(BookViewModel viewModel);

        BookViewModel ToBookViewModel(Book book);

        BookViewModel ToBookViewModel(BookAndBookEditionViewModel bbViewModel);

        BookEdition ToBookEdition(BookEditionViewModel viewModel);

        BookEditionViewModel ToBookEditionViewModel(BookEdition bookEdition);

        BookEditionViewModel ToBookEditionViewModel(BookAndBookEditionViewModel bbViewModel, Guid imageId);

        AuthorViewModel ToAuthorViewModel(Author author);

        BookPublisherViewModel ToBookPublisherViewModel(BookPublisher bookPublisher);
    }
}
