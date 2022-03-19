using BookLibrary.Infrastructure.Data;
using BookLibrary.Models;
using BookLibrary.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookLibrary.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext data;

        public HomeController(ApplicationDbContext _data)
        {
            data = _data;
        }

        public IActionResult Index()
        {
            var books = data
               .Books
               .OrderByDescending(b => b.Id)
               .Select(b => new AllBooksViewModel
               {
                   Title = b.Title,
                   ImageUrl = b.ImageUrl,
                    //Authors = (IEnumerable<AuthorImagesViewModel>)b.Authors,
                    //Genres = (ICollection<GenreType>)b.Genres
                })
               .Take(3)
               .ToList();

            return View(books);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}