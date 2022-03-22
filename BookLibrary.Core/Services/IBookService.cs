namespace BookLibrary.Core.Services
{
    public interface IBookService
    {
        BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage);
        // IEnumerable<BookServiceModel> MyBooks(string userId);
         Task AddToMyBooks(string bookId, string userId);
    }
}
