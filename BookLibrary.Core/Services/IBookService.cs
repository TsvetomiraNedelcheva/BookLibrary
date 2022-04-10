using BookLibrary.Infrastructure.Data.Models.Enums;

namespace BookLibrary.Core.Services
{
    public interface IBookService
    {
        public void Add(string title, string description, int pages, string imageUrl, string publisher, IEnumerable<AuthorImagesServiceModel> authors, ICollection<GenreType> genres);
        BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage);
         Task AddToMyBooks(string bookId, string userId);
        Task RemoveFromBookList(string bookId, string userId);
        BookEditDetailsServiceModel EditDetails(string id);

        void Edit(string id, string title, string description, string imageURL, int pages, string publisher, string authors, List<string> genres);
        void Delete(string id);

        BookDetailsServiceModel Details(string id);

        void LeaveReview(string bookId, string userId, string content);
    }
}
