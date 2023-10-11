using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data
{
    public interface IRentRepository
    {
        Task RentBookAsync(string userId, int bookId);
        Task<IEnumerable<Book>> GetRentalsByUserAsync(string userId);
        Task ReturnBookAsync(string userId, int bookId);
        Task CancelRentalAsync(string userId, int bookId);
    }

}
