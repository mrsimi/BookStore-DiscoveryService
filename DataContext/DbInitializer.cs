using BookStore_DiscoveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore_DiscoveryService.DataContext
{
    internal class DbInitializer
    {
        private ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        internal void Seed()
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre{Id = 1, Name = "Horror"},
                new Genre{Id = 2, Name = "Sci-Fi"},
                new Genre{Id = 3, Name = "Comedy"}
            );

             modelBuilder.Entity<Author>().HasData(
                new Author{Id = 1, FirstName = "Stephen", LastName = "King"},
                new Author{Id = 2, FirstName = "Stephen", LastName = "King"},
                new Author{Id = 3, FirstName = "Stephen", LastName = "King"}
            );

            modelBuilder.Entity<Book>().HasData(
                new Book{Id = 1, Title = "Carrie", Year = 1974, Price = 10.00m, AuthorId = 1, GenreId = 1},
                new Book{Id = 2, Title = "it", Year = 1986, Price = 20.00m, AuthorId = 1, GenreId = 1},
                new Book{Id = 3, Title = "The Shinning", Year = 1977, Price = 35.20m, AuthorId = 1, GenreId = 1}
            );

            
        }
    }
}