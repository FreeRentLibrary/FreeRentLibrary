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
using Microsoft.Extensions.Logging;

namespace FreeRentLibrary.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<SeedDB> _logger;

        public SeedDB(DataContext context, IUserHelper userHelper, ICountryRepository countryRepository, ILogger<SeedDB> logger)
        {
            _context = context;
            _userHelper = userHelper;
            _countryRepository = countryRepository;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
            
            await SeedRoles();

            await SeedBookTypes();

            await SeedDefaultGenres();

            await SeedCountriesApi();

            await SeedAdmin();

            await SeedDefaultAuthors();

            await SeedDefaultBooks();

            await _context.SaveChangesAsync();
            
        }

        private async Task SeedDefaultAuthors()
        {
            if (!_context.Authors.Any())
            {
                try
                {
                    await _context.Database.OpenConnectionAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Authors ON");

                    List<Author> authorsToAdd = new List<Author>
                    {
                        new Author {Id = 1 , Name = "William Shakespeare"},
                        new Author {Id = 2 , Name = "Fernando Pessoa"}
                    };
                    await _context.Authors.AddRangeAsync(authorsToAdd);
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Authors OFF");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred during data seeding for authors.");
                    throw;
                }
            }
        }

        private async Task SeedDefaultBooks()
        {
            if (!_context.Books.Any())
            {
                try
                {
                    await _context.Database.OpenConnectionAsync();
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Books ON");

                    List<Book> booksToAdd = new List<Book>
                    {
                        new Book 
                        {  
                            Id = 1, 
                            Name = "Macbeth",
                            AuthorId = 1,
                            NativeLanguage = "en",
                            Synopsis = "Macbeth is a tragedy by William Shakespeare. It is thought to have been first performed in 1606.It dramatises the damaging physical and psychological effects of political ambition on those who seek power.",
                        },

                        new Book 
                        {
                            Id = 2,
                            Name = "Romeo and Juliet",
                            AuthorId = 1,
                            NativeLanguage = "en",
                            Synopsis = "Romeo and Juliet is a tragedy written by William Shakespeare early in his career about the romance between two Italian youths from feuding families. It was among Shakespeare's most popular plays during his lifetime and, along with Hamlet, is one of his most frequently performed. Today, the title characters are regarded as archetypal young lovers.",
                        },

                        new Book
                        {
                            Id = 3,
                            Name = "Mensagem",
                            AuthorId = 2,
                            NativeLanguage = "pt",
                            Synopsis = "Mensagem is a book by Portuguese writer Fernando Pessoa. It is composed of 44 poems, and was called the \"livro pequeno de poemas\" or the \"little book of poems\". It was published in 1934 by Parceria António Maria Pereira. The book was awarded, in the same year, with the Prémio Antero de Quental in the poem category by the Secretariado Nacional de Informação of the Estado Novo."
                        }
                    };

                    await _context.Books.AddRangeAsync(booksToAdd);
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Books OFF");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred during data seeding for authors.");
                    throw;
                }
            }
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
