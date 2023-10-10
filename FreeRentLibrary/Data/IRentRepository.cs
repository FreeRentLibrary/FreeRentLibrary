using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data
{
    public interface IRentRepository
    {
        Task RentBookAsync(int userId, int bookId);
        Task<IEnumerable<Book>> GetRentalsByUserAsync(int userId);
        Task ReturnBookAsync(int userId, int bookId);
        Task CancelRentalAsync(int userId, int bookId);
    }

}
