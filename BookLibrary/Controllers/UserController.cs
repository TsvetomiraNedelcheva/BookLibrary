using BookLibrary.Core.Services;
using BookLibrary.Core.Services.ServiceModels;
using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class UserController : BaseController
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;
        public UserController(ApplicationDbContext _data, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IUserService _userService)
        {
            data = _data;
            userManager = _userManager;
            roleManager = _roleManager;
            userService = _userService;
        }
        public async Task<IActionResult> MyBooks(MyBooksViewModel model)      
        {
            var user = await userManager.GetUserAsync(this.User);
            var books = userService.MyBooks( user , model.CurrentPage, MyBooksViewModel.BooksPerPage);
            model.TotalBooks = books.TotalBooks; 
            model.UserName = user.UserName;
            model.FisrtName = user.FisrtName;
            model.LastName = user.LastName;
            model.Books = books.Books.Select(b => new BookServiceModel
            {
                Id = b.Id,
                Title = b.Title,
                Image = b.Image
            }).ToList();

            return View(model);
        }
    }
}
