using BookLibrary.Core.Services.ServiceModels;

namespace BookLibrary.Core.Services
{
    public interface IAuthorService
    {
        AuthorsQueryServiceModel GetAllAuthors(string searchTerm, int currentPage, int authorsPerPage);
        AuthorDetailsServiceModel AuthorDetails(string id);
    }
}
