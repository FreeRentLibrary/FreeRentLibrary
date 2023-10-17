using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IAuthorRepository:IGenericRepository<Author>
    {
        IQueryable GetAuthorsWithGenresAndBooks();

        Task AddAuthorWithGenresAsync(AuthorViewModel viewModel);

        Task DeleteAllAuthorInfoAsync(int authorId);

        Task<Author> GetAuthorWithGenresAndBooks(int authorId);

        Task<IEnumerable<Author>> GetAuthorsByGenreAsync(int genreId);

        Task<IEnumerable<Author>> GetAuthorsByGenreListAsync(IEnumerable<int> genreIdList);

        IEnumerable<SelectListItem> GetComboAuthors();

        IEnumerable<SelectListItem> GetComboAuthors(int authorId);

    }
}
