using BookLibrary.Infrastructure.Data.Models;

namespace BookLibrary.Core.Services
{
    public interface IBookService
    {
        BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage);
         Task AddToMyBooks(string bookId, string userId);
        BookDetailsServiceModel Details(string id);

        bool Edit(string id, string title, string description, string imageURL, int pages, Publisher publisher, ICollection<Author> authors, ICollection<Genre> genres);
    }
}
