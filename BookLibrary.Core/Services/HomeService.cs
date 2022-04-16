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
                   Image = b.BookImage.RemoteImageUrl,
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

            var booksCount = data.Books.Where(x=>x.IsDeleted == false).Count();
            var authorsCount = data.Authors.Where(a => a.Books.Any(b => b.IsDeleted == false)).Count();
            var reviewsCount = data.Reviews.Count();


            return new HomePageBookServiceModel
            {
                Books = books,
                BooksCount = booksCount,
                AuthorsCount = authorsCount,
                ReviewsCount = reviewsCount
            };
        }
    }
}
