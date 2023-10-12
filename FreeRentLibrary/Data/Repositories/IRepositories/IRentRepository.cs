using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IRentRepository
    {
        Task RentBookAsync(string userId, int libraryId, int bookId);
        Task<IEnumerable<Rent>> GetRentalsByUserAsync(string userId);
        Task ReturnBookAsync(string userId, int libraryId);
        Task CancelRentalAsync(string userId, int libraryId);
    }

}
