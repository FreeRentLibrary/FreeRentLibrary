﻿using FreeRentLibrary.Data;
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
        private readonly RentRepository _rentRepository;

        public ConverterHelper(DataContext context,RentRepository rentRepository)
        {
            _context = context;
            _rentRepository = rentRepository;
        }

        public BookEditionViewModel ToBookEditionViewModel(BookAndBookEditionViewModel bbViewModel, Guid imageId)
        {
            return new BookEditionViewModel
            {
                EditionName = bbViewModel.EditionName,
                BookId = bbViewModel.BookId,
                BookPublisherId = bbViewModel.BookPublisherId,
                BookPublishers = bbViewModel.BookPublisher,
                BookTypes = bbViewModel.BookType,
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

        public async Task ReserveToRentAsync(Reservation reservation)
        {
            if (reservation.UserId != null && reservation.LibraryId != null)
            {
                var userId = reservation.UserId;
                int bookId = reservation.BookEditionId.Value;
                int libraryId = reservation.LibraryId.Value;

                await _rentRepository.RentBookAsync(userId, libraryId, bookId);

                reservation.EndDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
