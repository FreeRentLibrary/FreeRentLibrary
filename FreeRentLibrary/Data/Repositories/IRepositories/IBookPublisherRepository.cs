using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IBookPublisherRepository:IGenericRepository<BookPublisher>
    {
        IQueryable GetPublishersWithCountry();

        Task<BookPublisher> GetPublisherWithBooksAndCountry(int publisherId);

        Task<IEnumerable<BookPublisher>> GetPublishersByCountryAsync(int countryId);
    }
}
