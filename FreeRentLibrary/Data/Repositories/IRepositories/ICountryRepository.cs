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
        IQueryable GetCountriesWithCities();
        Task<Country> GetCountryWithCityAsync(int id);
        Task<City> GetCityAsync(int id);
        Task AddCityAsync(CityViewModel model);
        Task<int> UpdateCityAsync(City city);
        Task<int> DeleteCityAsync(City city);
        IEnumerable<SelectListItem> GetComboCountries();
        IEnumerable<SelectListItem> GetComboCities(int countryId);
        Task<Country> GetCountryAsync(City city);
        Task<City> GetFirstCityByCountryNameAsync(string countryName);
        Task<Country> GetCountryByNameAsync(string countryName);
        Task AddCityListAsync(string countryName, List<City> cities);
    }
}
