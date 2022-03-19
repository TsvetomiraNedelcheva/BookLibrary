using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Infrastructure.Data.Models.Enums;
using BookLibrary.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext data;
        public BookController(ApplicationDbContext _data)
        {
            data = _data;
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

        public IActionResult All(string searchTerm, string genre)
        {
            var bookQuery = this.data.Books.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                bookQuery = bookQuery.Where(b => b.Title.ToLower().Contains(searchTerm.ToLower()));
            }
            var books = bookQuery
                .OrderByDescending(b  => b.Id)
                .Select(b => new AllBooksViewModel
                {
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    //Authors = (IEnumerable<AuthorImagesViewModel>)b.Authors,
                    //Genres = (ICollection<GenreType>)b.Genres
                })
                .ToList();

            if (!string.IsNullOrWhiteSpace(genre))
            {
                bookQuery = bookQuery.Where(b => b.Genres.Select(g => g.Name).ToString() == genre);
            }

            var bookGenres = data
                .Genres
                .Select(g => g.Name.ToString()) //enum is int by default and can't cast it to string
                .ToList();

            return View(new BookSearchViewModel
            {
                GenreTypes = bookGenres,
                Books = books,
                SearchTerm = searchTerm
            });
        }
    }
}
