using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IBookRepository:IGenericRepository<Book>
    {
        #region Book
        IQueryable GetBooksWithAuthorsAndGenres();

        Task AddBookAsync(BookViewModel viewModel);

        Task DeleteBookAsync(int bookId);

        bool CheckIfBookExists(string bookName, int authorId);

        Task<Book> GetBookWithNameAsync(string bookName);

        Task<Book> GetBookWithAllDataAsync(int bookId);

        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);

        Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId);

        #endregion

        //--

        #region BookEdition
        
        Task AddBookEditionAsync(BookEditionViewModel viewModel);

        Task<BookEdition> GetBookEditionAsync(int bookEditionId);

        #endregion

        //--

        #region Combo

        IEnumerable<SelectListItem> GetComboBooks();

        IEnumerable<SelectListItem> GetComboBookTypes();

        IEnumerable<SelectListItem> GetComboBookPublishers();

        #endregion
    }
}
