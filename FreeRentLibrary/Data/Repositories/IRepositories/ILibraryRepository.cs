using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface ILibraryRepository : IGenericRepository<Library>
    {

        IQueryable GetLibrariesWithCity();

        Task AddBookToStock(int bookEditionId, int libraryId, int quantity);

        Task<Library> GetLibraryWithAllInfo(int libraryId);

        Task<bool> CheckStockAsync(int libraryId, int bookId);
	}
}
