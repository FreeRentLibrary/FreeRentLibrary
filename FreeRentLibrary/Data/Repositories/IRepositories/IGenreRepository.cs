﻿using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IGenreRepository:IGenericRepository<Genre>
    {
        IQueryable GetGenresWithAuthorsAndBooks();

        Task<Genre> GetGenreWithAuthorsAndBooks(int genreId);

        IEnumerable<Genre> GetGenres(List<int> genresIdList);
    }
}
