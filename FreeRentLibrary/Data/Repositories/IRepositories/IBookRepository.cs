using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IBookRepository:IGenericRepository<Book>
    {
        IQueryable GetBooksWithAuthorsAndGenres();

        Task<Book> GetBookWithAllDataAsync(int bookId);

        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);

        Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId);

        Task<BookEdition> GetBookEditionAsync(int bookEditionId);
    }
}
