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
                //ImageId = imageId,
                //IsAvailable = model.IsAvailable,
            };
        }

        public BookViewModel ToBookViewModel(Book product)
        {
            return new BookViewModel
            {
                Id = product.Id,
                //IsAvailable = product.IsAvailable,
                //ImageId = product.ImageId,
            };
        }

        public AddBookEditionViewModel ToBookEditionViewModel(BookAndBookEditionViewModel bbViewModel)
        {
            return new AddBookEditionViewModel
            {
                EditionName = bbViewModel.EditionName,
                BookId = bbViewModel.BookId,
                BookPublisherId = bbViewModel.BookPublisherId,
                BookPublisher = bbViewModel.BookPublisher,
                BookType = bbViewModel.BookType,
                BookTypeId = bbViewModel.BookTypeId,
                ISBN = bbViewModel.ISBN,
                MinimumAge = bbViewModel.MinimumAge,
                PageCount = bbViewModel.PageCount,
                SameBookName = bbViewModel.SameBookName,
                ReleaseDate = bbViewModel.ReleaseDate,
                TranslatedLanguage = bbViewModel.TranslatedLanguage,
                Translator = bbViewModel.Translator,
            };
        }

        public AddBookViewModel ToBookViewModel(BookAndBookEditionViewModel bbViewModel)
        {
            return new AddBookViewModel
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
    }
}
