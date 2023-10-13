using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        #region Country

        IQueryable GetCountriesWithCities();

        Task<Country> GetCountryAsync(City city);
        
        Task<Country> GetCountryByNameAsync(string countryName);

        Task<Country> GetCountryWithCityAsync(int id);

        IEnumerable<SelectListItem> GetComboCountries();

        #endregion

        //--

        #region City
        
        Task AddCityAsync(CityViewModel model);
        
        Task AddCityListAsync(string countryName, List<City> cities);
        
        Task<City> GetCityAsync(int id);

        Task<City> GetFirstCityByCountryNameAsync(string countryName);
        
        Task<int> UpdateCityAsync(City city);
        
        Task<int> DeleteCityAsync(City city);
        
        IEnumerable<SelectListItem> GetComboCities(int countryId);

        #endregion
        
    }
}
