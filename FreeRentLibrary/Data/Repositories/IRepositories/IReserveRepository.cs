using FreeRentLibrary.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IReserveRepository
    {
        Task ReserveBookAsync(string userId, int libraryId);
        Task<IEnumerable<Reservation>> GetReservationsByUserAsync(string userId);
        Task ReserveToRentAsync(Reservation reservation);
        Task CancelReservationAsync(string userId, int libraryId);
    }

}
