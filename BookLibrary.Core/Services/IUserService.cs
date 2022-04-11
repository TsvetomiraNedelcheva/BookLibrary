using BookLibrary.Core.Services.ServiceModels;
using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Core.Services
{
    public interface IUserService
    {
        public MyBooksQueryServiceModel MyBooks(ApplicationUser user, int currentPage, int booksPerPage);
    }
}
