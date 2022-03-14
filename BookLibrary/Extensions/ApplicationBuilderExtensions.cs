using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;
            var data = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            SeedGenres(data);

            return app;
        }

        private static void SeedGenres(ApplicationDbContext data)
        {

            if (data.Genres.Any())
            {
                return;
            }

            data.Genres.AddRange(new[]
            {
                new Genre { Name = "Classics" },
                new Genre { Name = "Fiction" },
                new Genre { Name = "Non-fiction" },
                new Genre { Name = "Science" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Thriller" },
                new Genre { Name = "Fantasy" },
                new Genre { Name = "Biography" },
               
            });

            data.SaveChanges();
        }
    }
}
