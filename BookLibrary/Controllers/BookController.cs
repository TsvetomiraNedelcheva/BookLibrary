using BookLibrary.Core.Services;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Models.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IBookService bookService;
        public BookController(ApplicationDbContext _data, IBookService _bookService)
        {
            data = _data;
            bookService = _bookService;
        }
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(AddBookFormModel book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            var genresList = new List<Genre>();
            foreach (var item in book.Genres)
            {
                genresList.Add(new Genre { Name = item });
            }

            var newBook = new Book
            {
                Title = book.Title,
                Description = book.Description,
                Pages = book.Pages,
                ImageUrl = book.ImageUrl,
                Publisher = new Publisher { Name = book.Publisher },
                Genres = genresList,
            };

            foreach (var inputAuthor in book.Authors)
            {
                var author = data.Authors.FirstOrDefault(a => a.Name == inputAuthor.Name);
                if (author == null)
                {
                    author = new Author()
                    {
                        Name = inputAuthor.Name,
                        ImageUrl = inputAuthor.AuthorImageUrl
                    };
                }
                data.Authors.Add(author);
                newBook.Authors.Add(author);
            }

            data.Books.Add(newBook);  
            data.SaveChanges();


            return RedirectToAction("Index", "Home");
        }

        public IActionResult All([FromQuery]AllBooksQueryModel query)
        {
            var books = bookService.All(query.SearchTerm, query.CurrentPage, AllBooksQueryModel.BooksPerPage);

            query.TotalBooks = books.TotalBooks;
            query.Books = books.Books.Select(b => new AllBooksViewModel
            {
                Id = b.Id,
                Title = b.Title,
                ImageUrl = b.ImageUrl,
            });

            return View(query);
        }
    }
}
