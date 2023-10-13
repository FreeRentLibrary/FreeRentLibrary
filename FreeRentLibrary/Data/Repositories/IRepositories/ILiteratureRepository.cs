using FreeRentLibrary.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface ILiteratureRepository
    {
        #region Author Related

        IQueryable GetAuthorsWithGenresAndBooks();

        Task<Author> GetAuthorWithGenresAndBooks(int authorId);

        Task<IEnumerable<Author>> GetAuthorsByGenreAsync (int genreId);

        Task<IEnumerable<Author>> GetAuthorsByGenreListAsync (IEnumerable<int> genreIdList);

        #endregion

        //--

        #region Book Related

        IQueryable GetBooksWithAuthorsAndGenres();

        Task<Book> GetBookWithAllDataAsync(int bookId);

        Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId);

        Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId);

        Task<BookEdition> GetBookEditionAsync(int bookEditionId);
        #endregion

        //--

        #region Genre Related

        IQueryable GetGenresWithAuthorsAndBooks();

        Task<Genre> GetGenreWithAuthorsAndBooks(int genreId);

        #endregion

        //--

        #region Publisher Related

        IQueryable GetPublishersWithCountry();

        Task<Publisher> GetPublisherWithBooksAndCountry(int publisherId);

        Task<IEnumerable<Publisher>> GetPublishersByCountryAsync (int countryId);

        #endregion


    }
}
