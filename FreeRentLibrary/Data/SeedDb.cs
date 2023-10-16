using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeRentLibrary.Data.API;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Helpers.SimpleHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FreeRentLibrary.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICountryRepository _countryRepository;
        
        public SeedDB(DataContext context, IUserHelper userHelper, ICountryRepository countryRepository)
        {
            _context = context;
            _userHelper = userHelper;
            _countryRepository = countryRepository;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
            
            await SeedRoles();

            await SeedBookTypes();

            await SeedDefaultGenres();

            await SeedCountriesApi();

            await SeedAdmin();

            await _context.SaveChangesAsync();
            
        }

        public async Task SeedRoles()
        {
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Reader");
        }

        public async Task SeedBookTypes()
        {
            if (!_context.BookTypes.Any())
            {
                await _context.BookTypes.AddAsync(new BookTypes { Name = "Hardcover" });
                await _context.BookTypes.AddAsync(new BookTypes { Name = "Paperback" });
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedDefaultGenres()
        {
            if (!_context.Genres.Any())
            {
                List<Genre> genresToAdd = new List<Genre>
                {
                    new Genre { Name = "Action and Adventure" },
                    new Genre { Name = "Biography" },
                    new Genre { Name = "Children's" },
                    new Genre { Name = "Classic" },
                    new Genre { Name = "Crime" },
                    new Genre { Name = "Drama" },
                    new Genre { Name = "Fantasy" },
                    new Genre { Name = "Historical Fiction" },
                    new Genre { Name = "Horror" },
                    new Genre { Name = "Mystery" },
                    new Genre { Name = "Non-Fiction" },
                    new Genre { Name = "Paranormal" },
                    new Genre { Name = "Poetry" },
                    new Genre { Name = "Romance" },
                    new Genre { Name = "Science Fiction" },
                    new Genre { Name = "Self-Help" },
                    new Genre { Name = "Suspense" },
                    new Genre { Name = "Thriller" },
                    new Genre { Name = "Travel" },
                    new Genre { Name = "Western" }
                };
                await _context.Genres.AddRangeAsync(genresToAdd);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedAdmin()
        {
            var user = await _userHelper.GetUserByEmailAsync("FreeRentLibrary@gmail.com");
            if (user == null)
            {
                var city = await _countryRepository.GetFirstCityByCountryNameAsync("Portugal");

                user = new User
                {
                    FirstName = "FreeRent",
                    LastName = "Library",
                    Email = "FreeRentLibrary@gmail.com",
                    UserName = "FreeRentLibrary@gmail.com",
                    PhoneNumber = "936232511",
                    Address = "Rua da Cruz Vermelha",
                    City = city,
                    CityId = city.Id
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user  in seeder");
                }
                
                await _userHelper.AddUserToRoleAsync(user, "Admin");
                
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
        }

        public async Task SeedCountriesApi() 
        {
            if (!_context.Countries.Any())
            {
                //tries to get a list of countries from the API
                var response = await ApiService.GetCountries();

                if (response.IsSuccess)
                {
                    var jCountries = (List<JCountry>)response.Results;

                    var cities = new List<City>();

                    foreach (var country in jCountries)
                    {
                        if (await _countryRepository.GetCountryByNameAsync(country.Name) == null)
                        {
                            _context.Countries.Add(new Country
                            {
                                Name = country.Name
                            });
                            await _context.SaveChangesAsync();

                            if (country.States != null && country.States.Count != 0)
                            {
                                foreach (var city in country.States)
                                {
                                    cities.Add(new City { Name = city.Name });
                                }
                            }
                            else if(country.States.Count == 0)
                            {
                                cities.Add(new City { Name = country.Name });
                            }

                            await _countryRepository.AddCityListAsync(country.Name, cities);

                            cities.Clear();
                        }
                    }
                }
                else
                {
                    var cities = new List<City>();
                    cities.Add(new City { Name = "Lisboa" });
                    cities.Add(new City { Name = "Porto" });
                    cities.Add(new City { Name = "Faro" });

                    _context.Countries.Add(new Country
                    {
                        Cities = cities,
                        Name = "Portugal"
                    });

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
