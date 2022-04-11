using BookLibrary.Core.Services.ServiceModels;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BookLibrary.Core.Services
{
    public  class UserService:IUserService
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(ApplicationDbContext _data, UserManager<ApplicationUser> _userManager )
        {
            data = _data;
            userManager = _userManager;
        }

        public MyBooksQueryServiceModel MyBooks(ApplicationUser user, int currentPage, int booksPerPage)
        {
            var booksQuery = data.ApplicationUsers.Where(x => x.Id == user.Id).SelectMany(x => x.Books).Where(b => b.IsDeleted == false).AsQueryable();

            var totalBooks = booksQuery.Count();


            var books = booksQuery
                .Skip((currentPage - 1) * booksPerPage)
                .Take(booksPerPage)
                .Select(b => new BookServiceModel
                {
                    Id = b.Id,
                    ImageUrl = b.ImageUrl,
                    Title = b.Title,
                })
                .ToList();

            return new MyBooksQueryServiceModel
            {
                UserName = user.UserName,
                FisrtName = user.FisrtName,
                LastName = user.LastName,
                TotalBooks = totalBooks,
                CurrentPage = currentPage,
                BooksPerPage = booksPerPage,
                Books = books
            };

        }
    }
}
