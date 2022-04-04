using BookLibrary.Core.Services;
using BookLibrary.Core.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class AuthorController:Controller
    {
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService _authorService)
        {
            authorService = _authorService;
        }

        public async Task<IActionResult> AllAuthors([FromQuery]AuthorsQueryServiceModel query)
        {
            var authors = authorService.GetAllAuthors(query.SearchTerm, query.CurrentPage, AuthorsQueryServiceModel.AuthorsPerPage);
            query.TotalAuthors = authors.TotalAuthors;
            query.Authors = authors.Authors.Select(a => new AllAuthorsServiceModel
            {
                Id = a.Id,
                Name = a.Name,
                ImageUrl = a.ImageUrl,
            });

            return View(query);
        }

        public async Task<IActionResult> AuthorDetails(string id)
        {
            var author = authorService.AuthorDetails(id);

            return View(new AuthorDetailsServiceModel
            {
                Id = author.Id,
                Name = author.Name,
                ImageUrl = author.ImageUrl,
                Books = author.Books
            });
        }
    }
}
