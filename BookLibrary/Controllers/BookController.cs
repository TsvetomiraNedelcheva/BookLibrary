using BookLibrary.Core.Services;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Infrastructure.Data.Models.Enums;
using BookLibrary.Models.Books;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IBookService bookService;
        private readonly UserManager<ApplicationUser> userManager;

        public BookController(ApplicationDbContext _data, IBookService _bookService , UserManager<ApplicationUser> _userManager)
        {
            data = _data;
            bookService = _bookService;
            userManager = _userManager;
        }
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(BookFormModel book)
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

        [HttpPost]
        public async Task<IActionResult> AddToMyBookList(AllBooksViewModel model)
        {
            var user = await userManager.GetUserAsync(this.User);
            var userId = await userManager.GetUserIdAsync(user);

            await bookService.AddToMyBooks(model.Id, userId);

            return RedirectToAction("MyBooks", "User");
        }

        public IActionResult Edit(string id)
        {
            var book = bookService.Details(id);

            //var authorsListMapped = new List<AuthorImagesViewModel>();
            //foreach (var author in book.Authors)
            //{
            //    authorsListMapped.Add(new AuthorImagesViewModel
            //    {
            //        Name = author.Name,
            //        AuthorImageUrl = author.AuthorImageUrl,
            //    });
            //}

            string authorsString = "";
            foreach (var author in book.Authors)
            {
                authorsString += author.Name + ",";
            }

            var genresListMapped = new List<string>();
            foreach (var genres in book.Genres)
            {
                genresListMapped.Add(genres.Name.ToString());
            }

            var genreTypeList = genresListMapped.Select(x => Enum.Parse<GenreType>(x)).ToList();

            return View(new EditBookFormModel
            {
                Title = book.Title,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Pages = book.Pages,
                Authors = authorsString,
                Publisher = book.Publisher,
                Genres = genreTypeList
            });
        }

        [HttpPost]
        public IActionResult Edit(string id, EditBookFormModel book)
        {
            var authorsString  = "";
            var bookData = data.Books.FirstOrDefault(x => x.Id == id); //the book
            //var bookAuthors = data.Books.Where(x => x.Id == id).SelectMany(x => x.Authors).ToList(); //bookAuthors
            var bookAuthors = book.Authors.Split(",").ToList();
            foreach (var author in bookAuthors)
            {
                authorsString += author + ",";
            }

            var genresList = new List<string>();
            foreach (var genre in book.Genres)
            {
                genresList.Add(genre.ToString());
            }

            bookService.Edit(id, book.Title, book.Description, book.ImageUrl, book.Pages, book.Publisher, authorsString.TrimEnd(','), genresList);
            return RedirectToAction("All", "Book");
        }

    }
}
