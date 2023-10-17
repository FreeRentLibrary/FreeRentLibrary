using FreeRentLibrary.Data;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public Book ToBook(BookViewModel viewModel)
        {
            return new Book
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                AuthorId = viewModel.AuthorId,
                Author = viewModel.Author,
                NativeLanguage = viewModel.NativeLanguage,
                BookEditions = viewModel.BookEditions,
                Synopsis = viewModel.Synopsis,
                BookGenres = viewModel.BookGenres,
            };
        }

        public BookViewModel ToBookViewModel(Book book)
        {
            List<int> selectedGenres = new List<int>();
            foreach (var ag in book.BookGenres)
            {
                selectedGenres.Add(ag.GenreId.Value);
            }

            var genres = _context.Genres.Where(g => selectedGenres.Contains(g.Id)).ToList();

            return new BookViewModel
            {
                Id = book.Id,
                Name = book.Name,
                AuthorId = book.AuthorId,
                Author = book.Author,
                Genres = genres,
                NativeLanguage = book.NativeLanguage,
                BookGenres = book.BookGenres,
                Synopsis = book.Synopsis,
                BookEditions = book.BookEditions,
                SelectedGenres = selectedGenres,
            };
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

        public AuthorViewModel ToAuthorViewModel(Author author)
        {
            List<int> selectedGenres = new List<int>();
            foreach (var ag in author.AuthorGenres)
            {
                selectedGenres.Add(ag.GenreId.Value);
            }

            var genres = _context.Genres.Where(g => selectedGenres.Contains(g.Id)).ToList();

            //selectedGenres = author.AuthorGenres.Select(g => g.Id).ToList();

            return new AuthorViewModel
            {
                Id = author.Id,
                Name = author.Name,
                AuthorPhotoId = author.AuthorPhotoId,
                Books = author.Books,
                Genres = genres,
                SelectedGenres = selectedGenres,
                AuthorGenres = author.AuthorGenres,
            };
        }



    }
}
