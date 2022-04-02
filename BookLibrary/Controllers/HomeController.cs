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
               .Where(b=>b.IsDeleted == false)
               .OrderBy(b => b.Title)
               .Select(b => new AllBooksViewModel
               {
                   Id = b.Id,
                   Title = b.Title,
                   ImageUrl = b.ImageUrl,
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