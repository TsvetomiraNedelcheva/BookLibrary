using BookLibrary.Core.Services.ServiceModels;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Core.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext data;

        public HomeService(ApplicationDbContext _data)
        {
            data = _data;
        }

        public HomePageBookServiceModel HomePage(List<Book> currentUserBooks)
        {

            var books = data
               .Books
               .Where(b => b.IsDeleted == false)
               .OrderBy(b => b.Title)
               .Select(b => new HomeBookServiceModel
               {
                   Id = b.Id,
                   Title = b.Title,
                   ImageUrl = b.ImageUrl,
               })
               .Take(3)
               .ToList();

            foreach (var book in books)
            {
                var userBook = currentUserBooks.Where(x => x.Id == book.Id).FirstOrDefault();
                if (userBook != null)
                {
                    book.IsAvailableToAddByUser = false;
                }
            }

            return new HomePageBookServiceModel
            {
                Books = books
            };
        }
    }
}
