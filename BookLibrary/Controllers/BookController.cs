using BookLibrary.Core.Services;
using BookLibrary.Core.Services.ServiceModels;
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

        public BookController(ApplicationDbContext _data, IBookService _bookService, UserManager<ApplicationUser> _userManager)
        {
            data = _data;
            bookService = _bookService;
            userManager = _userManager;
        }
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(BookFormModel book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
           await bookService.Add(book.Title, book.Description, book.Pages, book.Image, book.Publisher, book.Authors, book.Genres);

            return RedirectToAction("All", "Book");
        }

        public IActionResult All([FromQuery] AllBooksQueryModel query)
        {
            var userId =  userManager.GetUserId(this.User);
            var currentUserBooks = data.Users.Where(x=>x.Id == userId).SelectMany(x=>x.Books).ToList();
            var books = bookService.All(query.SearchTerm, query.CurrentPage,
                AllBooksQueryModel.BooksPerPage,currentUserBooks);

            query.TotalBooks = books.TotalBooks;
            query.Books = books.Books.Select(b => new AllBooksViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Image = b.Image,
                IsAvailableToAddByUser = b.IsAvailableToAddByUser,
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

        [HttpPost]
        public async Task<IActionResult> RemoveFromBookList(string id)
        {
            var user = await userManager.GetUserAsync(this.User);
            var userId = await userManager.GetUserIdAsync(user);

            await bookService.RemoveFromBookList(id, userId);

            return RedirectToAction("MyBooks", "User");
        }

        public IActionResult Edit(string id)
        {
            var book = bookService.EditDetails(id);
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
                Pages = book.Pages,
                Authors = authorsString,
                Publisher = book.Publisher,
                Genres = genreTypeList
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditBookFormModel book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
            var authorsString = "";
            var bookData = data.Books.FirstOrDefault(x => x.Id == id); //the book
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

            await bookService.Edit(id, book.Title, book.Description, book.Image, book.Pages, book.Publisher, authorsString.TrimEnd(','), genresList);
            return RedirectToAction("All", "Book");
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            bookService.Delete(id);
            return RedirectToAction("All", "Book");
        }

        public IActionResult Details(string id)
        {
            var book = bookService.Details(id);

            return View(new BookDetailsServiceModel
            {
                Title = book.Title,
                Description = book.Description,
                Image = book.Image,
                Pages = book.Pages,
                Authors = book.Authors,
                Publisher = book.Publisher,
                Users = book.Users,
                Reviews = book.Reviews,
                Genres = book.Genres
            });
        }

        public async Task<IActionResult> Review()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Review(string id, AddReviewServiceModel model)
        {
            var user = await userManager.GetUserAsync(this.User);

            bookService.LeaveReview(id, user.Id, model.Content);
            return RedirectToAction("Details", new { id = id });

        }
    }
}
