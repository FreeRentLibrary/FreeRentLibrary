using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FreeRentLibrary.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;

        public ConverterHelper(DataContext context)
        {
            _context = context;
        }

        public BookViewModel ToBookViewModel(BookAndBookEditionViewModel bbViewModel)
        {
            return new BookViewModel
            {
                AuthorId = bbViewModel.AuthorId.Value,
                Authors = bbViewModel.Authors,
                Genres = bbViewModel.Genres,
                Name = bbViewModel.Name,
                NativeLanguage = bbViewModel.NativeLanguage,
                SelectedGenres = bbViewModel.SelectedGenres,
                Synopsis = bbViewModel.Synopsis,
            };
        }

        public BookEdition ToBookEdition(BookEditionViewModel viewModel)
        {
            return new BookEdition
            {
                Id = viewModel.Id,
                EditionName = viewModel.EditionName,
                BookId = viewModel.BookId,
                Book = viewModel.Book,
                BookPublisherId = viewModel.BookPublisherId,
                BookPublisher = viewModel.BookPublisher,
                BookTypeId = viewModel.BookTypeId,
                BookType = viewModel.BookType,
                ISBN = viewModel.ISBN,
                MinimumAge = viewModel.MinimumAge,
                PageCount = viewModel.PageCount,
                ReleaseDate = viewModel.ReleaseDate,
                TranslatedLanguage = viewModel.TranslatedLanguage,
                Translator = viewModel.Translator,
                CoverId = viewModel.CoverId,
            };
        }

        public BookEditionViewModel ToBookEditionViewModel(BookEdition bookEdition)
        {
            return new BookEditionViewModel
            {
                Id = bookEdition.Id,
                EditionName = bookEdition.EditionName,
                BookId = bookEdition.BookId,
                Book = bookEdition.Book,
                BookPublisherId = bookEdition.BookPublisherId,
                BookPublisher = bookEdition.BookPublisher,
                BookTypeId = bookEdition.BookTypeId,
                BookType = bookEdition.BookType,
                ISBN = bookEdition.ISBN,
                MinimumAge = bookEdition.MinimumAge,
                PageCount = bookEdition.PageCount,
                ReleaseDate = bookEdition.ReleaseDate,
                TranslatedLanguage = bookEdition.TranslatedLanguage,
                Translator = bookEdition.Translator,
                CoverId = bookEdition.CoverId,
            };
        }

        public BookEditionViewModel ToBookEditionViewModel(BookAndBookEditionViewModel bbViewModel, Guid imageId)
        {
            return new BookEditionViewModel
            {
                EditionName = bbViewModel.EditionName,
                BookId = bbViewModel.BookId,
                BookPublisherId = bbViewModel.BookPublisherId,
                BookPublishers = bbViewModel.BookPublisher,
                BookTypes = bbViewModel.BookTypes,
                BookTypeId = bbViewModel.BookTypeId,
                ISBN = bbViewModel.ISBN,
                MinimumAge = bbViewModel.MinimumAge,
                PageCount = bbViewModel.PageCount,
                SameBookName = bbViewModel.SameBookName,
                ReleaseDate = bbViewModel.ReleaseDate,
                TranslatedLanguage = bbViewModel.TranslatedLanguage,
                Translator = bbViewModel.Translator,
                CoverId = imageId,
            };
        }

    }
}
