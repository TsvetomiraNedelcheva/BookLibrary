using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Core.Services
{
    public interface IBookService
    {
        BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage);
         Task AddToMyBooks(string bookId, string userId);
        BookEditDetailsServiceModel EditDetails(string id);

        void Edit(string id, string title, string description, string imageURL, int pages, string publisher, string authors, List<string> genres);
        void Delete(string id);

        BookDetailsServiceModel Details(string id);
    }
}
