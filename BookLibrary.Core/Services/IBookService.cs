using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace BookLibrary.Core.Services
{
    public interface IBookService
    {
        public Task Add(string title, string description, int pages, IFormFile image, string publisher, IEnumerable<AuthorImagesServiceModel> authors, ICollection<GenreType> genres);
        BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage,List<Book> currentUserBooks);
         Task AddToMyBooks(string bookId, string userId);
        Task RemoveFromBookList(string bookId, string userId);
        BookEditDetailsServiceModel EditDetails(string id);

        Task Edit(string id, string title, string description, IFormFile image, int pages, string publisher, string authors, List<string> genres);
        void Delete(string id);

        BookDetailsServiceModel Details(string id);

        void LeaveReview(string bookId, string userId, string content);
    }
}
