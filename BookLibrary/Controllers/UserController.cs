using BookLibrary.Infrastructure.Data;
using BookLibrary.Infrastructure.Data.Models;
using BookLibrary.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly UserManager<ApplicationUser> userManager;
        public UserController(ApplicationDbContext _data, UserManager<ApplicationUser> _userManager)
        {
            data = _data;
            userManager = _userManager;
        }
        public async Task<IActionResult> MyBooks()      
        {
            var user = await userManager.GetUserAsync(this.User);

            var vm = new UserViewModel
            {
                UserName = user.UserName,
                FisrtName = user.FisrtName,
                LastName = user.LastName,
                Books = user.Books
            };

            return View(vm);
        }
    }
}
