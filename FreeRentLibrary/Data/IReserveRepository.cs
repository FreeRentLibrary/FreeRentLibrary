using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data
{
	public interface IReserveRepository
    {
        Task ReserveBookAsync(string userId, int bookId);
        Task<IEnumerable<Book>> GetReservationsByUserAsync(string userId);
        Task ReserveToRentAsync(Reservation reservation);
        Task CancelReservationAsync(string userId, int bookId);
    }

}
