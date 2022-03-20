namespace BookLibrary.Core.Services
{
    public interface IBookService
    {
        BookQueryServiceModel All(string searchTerm, int currentPage, int booksPerPage);
    }
}
