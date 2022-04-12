using BookLibrary.Core.Services;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Models;
using BookLibrary.Models.Books;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookLibrary.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext data;
        private readonly IHomeService homeService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ApplicationDbContext _data, IHomeService _homeService, UserManager<ApplicationUser> _userManager)
        {
            data = _data;
            homeService = _homeService;
            userManager = _userManager;
        }

        public IActionResult Index(HomePageBooksViewModel model)
        {
            var userId = userManager.GetUserId(this.User);
            var currentUserBooks = data.Users.Where(x => x.Id == userId).SelectMany(x => x.Books).ToList();

            var books = homeService.HomePage(currentUserBooks);

            model.Books = books.Books.Select(b => new AllBooksViewModel
            {
                Id = b.Id,
                Title = b.Title,
                ImageUrl = b.ImageUrl,
                IsAvailableToAddByUser = b.IsAvailableToAddByUser,
            });

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}