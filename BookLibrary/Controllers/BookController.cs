using BookLibrary.Infrastructure.Data;
using BookLibrary.Models.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext data;
        public BookController(ApplicationDbContext _data)
        {
            data = _data;
        }
        public IActionResult Add() => View(new AddBookFormModel
        {
            Genres = this.GetGenres()
        });

        [HttpPost]
        public IActionResult Add(AddBookFormModel book)
        {
            return View();
        }
        private IEnumerable<BookGenreViewModel> GetGenres()
        => data.Genres.Select(g => new BookGenreViewModel
        {
            Id = g.Id,
            Name = g.Name
        })
         .ToList();
    }
}
