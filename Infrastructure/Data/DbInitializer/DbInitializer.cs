using Domain.Entities.Books;
using Domain.Entities.Companies;
using Domain.Entities.Identity;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _dbContext;

        public DbInitializer(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                // Apply migrations
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }

                // Seed roles
                if (!_roleManager.RoleExistsAsync(UserRole.Customer.ToString()).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new ApplicationRole(UserRole.Admin.ToString())).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new ApplicationRole(UserRole.Customer.ToString())).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new ApplicationRole(UserRole.Employee.ToString())).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new ApplicationRole(UserRole.Company.ToString())).GetAwaiter().GetResult();
                }

                // Seed admin user
                if (!_dbContext.ApplicationUsers.Any())
                {
                    var user = new ApplicationUser
                    {
                        Name = "Zakariae Naaim",
                        UserName = "Zakariaenaaim",
                        Email = "admin@gmail.com",
                        PhoneNumber = "123456789",
                        AddressInfo = new Domain.Entities.Common.AddressInfo { StreetAddress = "123 Main St" ,
                            City = "Tostedt",
                            State = "Germany",
                            PostalCode = "21255",
                        },
                        LockoutEnabled = false
                    };

                    var result = _userManager.CreateAsync(user, "Admin123@").GetAwaiter().GetResult();

                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, UserRole.Admin.ToString()).GetAwaiter().GetResult();
                    }
                }

                // Seed categories/books/images
                if (!_dbContext.Categories.Any())
                {
                    _dbContext.Categories.AddRange(_SeedCategory());
                    _dbContext.SaveChanges();

                    _dbContext.Books.AddRange(_SeedBooks());
                    _dbContext.SaveChanges();

                    _dbContext.BookImages.AddRange(_SeedBookImages());
                    _dbContext.SaveChanges();
                }

                // Seed companies
                if (!_dbContext.Companies.Any())
                {
                    _dbContext.Companies.AddRange(_SeedCompanies());
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
                throw;
            }
        }

        private static List<TbCategory> _SeedCategory() => new()
        {
            new TbCategory { Name = "Action", DisplayOrder = 1 },
            new TbCategory { Name = "SciFi", DisplayOrder = 2 },
            new TbCategory { Name = "History", DisplayOrder = 3 }
        };

        private static List<TbBook> _SeedBooks() => new()
        {
            new TbBook
            {
                Title = "Fortune of Time",
                Author = "Billy Spark",
                ISBN = "SWD-9999-001A",
                ListPrice = 99,
                Price = 90,
                Price50 = 85,
                Price100 = 80,
                CategoryId = 1,
                Description = "An inspiring journey through time and destiny."
            },
            new TbBook
            {
                Title = "Dark Skies",
                Author = "Nancy Hoover",
                ISBN = "CAW-7777-01B",
                ListPrice = 40,
                Price = 30,
                Price50 = 25,
                Price100 = 20,
                CategoryId = 1,
                Description = "A thrilling mystery set under ominous skies."
            },
            new TbBook
            {
                Title = "Vanish in the Sunset",
                Author = "Julian Button",
                ISBN = "RIT-0555-01C",
                ListPrice = 55,
                Price = 50,
                Price50 = 40,
                Price100 = 35,
                CategoryId = 2,
                Description = "A romantic tale of love fading with the sunset."
            },
            new TbBook
            {
                Title = "Cotton Candy",
                Author = "Abby Muscles",
                ISBN = "WS3-3333-3301D",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = 3,
                Description = "A sweet and colorful story full of joy."
            },
            new TbBook
            {
                Title = "Rock in the Ocean",
                Author = "Ron Parker",
                ISBN = "SOT-1111-1101E",
                ListPrice = 30,
                Price = 27,
                Price50 = 25,
                Price100 = 20,
                CategoryId = 2,
                Description = "An adventure of resilience against the waves."
            },
            new TbBook
            {
                Title = "Leaves and Wonders",
                Author = "Laura Phantom",
                ISBN = "FOT-0000-0001F",
                ListPrice = 25,
                Price = 23,
                Price50 = 22,
                Price100 = 20,
                CategoryId = 2,
                Description = "A poetic exploration of nature’s mysteries."
            }
        };

        private static List<TbBookImage> _SeedBookImages() => new()
        {
            new TbBookImage { ImageUrl = "assets/images/books/book-1/fortune of time.jpg", BookId = 1, IsMainImage = true },
            new TbBookImage { ImageUrl = "assets/images/books/book-1/fortune of time back.jpg", BookId = 1, IsMainImage = false },

            new TbBookImage { ImageUrl = "assets/images/books/book-2/dark skies.jpg", BookId = 2, IsMainImage = true },
            new TbBookImage { ImageUrl = "assets/images/books/book-2/dark skies back.jpg", BookId = 2, IsMainImage = false },

            new TbBookImage { ImageUrl = "assets/images/books/book-3/vanish in the sunset.jpg", BookId = 3, IsMainImage = true },
            new TbBookImage { ImageUrl = "assets/images/books/book-3/vanish in the sunset back.jpg", BookId = 3, IsMainImage = false },

            new TbBookImage { ImageUrl = "assets/images/books/book-4/cotton candy.jpg", BookId = 4, IsMainImage = true },
            new TbBookImage { ImageUrl = "assets/images/books/book-4/cotton candy back.jpg", BookId = 4, IsMainImage = false },

            new TbBookImage { ImageUrl = "assets/images/books/book-5/rock in the ocean.jpg", BookId = 5, IsMainImage = true },
            new TbBookImage { ImageUrl = "assets/images/books/book-5/rock in the ocean back.jpg", BookId = 5, IsMainImage = false },

            new TbBookImage { ImageUrl = "assets/images/books/book-6/leaves and wonders.jpg", BookId = 6, IsMainImage = true },
            new TbBookImage { ImageUrl = "assets/images/books/book-6/leaves and wonder back.jpg", BookId = 6, IsMainImage = false }
        };


        private static List<TbCompany> _SeedCompanies() => new()
        {
            new TbCompany { Name = "Tech Innovations", PhoneNumber = "30282123", AddressInfo = new Domain.Entities.Common.AddressInfo{ City = "Hamburg", State = "Germany", StreetAddress = "123 Tech Street", PostalCode = "10001" } },
            new TbCompany { Name = "Global Solutions", PhoneNumber = "40011255",AddressInfo = new Domain.Entities.Common.AddressInfo{ City = "New York", State = "NY", StreetAddress = "456 Global Ave", PostalCode = "10002" } },
            new TbCompany { Name = "Future Enterprises", PhoneNumber = "50077777",AddressInfo = new Domain.Entities.Common.AddressInfo{ City = "Berlin", State = "Berlin", StreetAddress = "789 Future Blvd", PostalCode = "10003" } }
        };
    }
}