using BookLibrary.Core.Services.ServiceModels;
using BookLibrary.Infrastructure.Data;

namespace BookLibrary.Core.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext data;
        public AuthorService(ApplicationDbContext _data)
        {
            data = _data;
        }
        public AuthorsQueryServiceModel GetAllAuthors(string searchTerm, int currentPage, int authorsPerPage)
        {

            var authorQuery = this.data.Authors.AsQueryable();

            var totalAuthors = authorQuery.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                authorQuery = authorQuery.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            var authors = authorQuery
                .Skip((currentPage - 1) * authorsPerPage)
                .Take(authorsPerPage)
                .OrderByDescending(b => b.Id)
                .Select(a => new AllAuthorsServiceModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    ImageUrl = a.ImageUrl,
                })
                .ToList();

            return new AuthorsQueryServiceModel
            {
                TotalAuthors = totalAuthors,
                CurrentPage = currentPage,
               Authors = authors
            };

            //var authors = data.Authors
            //    .Select(a => new AllAuthorsServiceModel
            //    {
            //        Id = a.Id,
            //        Name = a.Name,
            //        ImageUrl = a.ImageUrl
            //    }).ToList();

            //return authors;
        }
    }
}
