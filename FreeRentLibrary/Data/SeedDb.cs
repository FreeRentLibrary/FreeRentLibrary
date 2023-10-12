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
        private Random _random;
        public SeedDB(DataContext context, IUserHelper userHelper, ICountryRepository countryRepository)
        {
            _context = context;
            _userHelper = userHelper;
            _countryRepository = countryRepository;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
            
            await SeedRoles();

            //await SeedCountry();

            await SeedCountriesApi();

            await SeedAdmin();

            await _context.SaveChangesAsync();
            

            /*if (!_context.Books.Any())
            {
                AddBook("Iphone X", user);
                AddBook("Magic Mause", user);
                AddBook("Iwatch", user);
                AddBook("Ipad mini", user);
                await _context.SaveChangesAsync();
            }*/
        }

        public async Task SeedRoles()
        {
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Reader");
        }

        public async Task SeedCountry()
        {
            if (!_context.Countries.Any())
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

        /*private void AddBook(string name, User user)
        {
            _context.Books.Add(new Book
            {
                Title = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user
            });
        }*/

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

                            if (country.States != null)
                            {
                                foreach (var city in country.States)
                                {
                                    cities.Add(new City { Name = city.Name });
                                }
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
