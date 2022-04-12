using BookLibrary.Core.Services.ServiceModels;
using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Core.Services
{
    public interface IHomeService
    {
        public HomePageBookServiceModel HomePage(List<Book> currentUserBooks);
    }
}
