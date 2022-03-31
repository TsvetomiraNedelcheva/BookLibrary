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
        public UserController(ApplicationDbContext _data, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            data = _data;
            userManager = _userManager;
            roleManager = _roleManager;
        }
        public async Task<IActionResult> MyBooks()      
        {
            var user = await userManager.GetUserAsync(this.User);
            var userBooks = data.ApplicationUsers.Where(x => x.Id == user.Id).SelectMany(x => x.Books).Where(b => b.IsDeleted == false).ToList();

            var vm = new UserViewModel
            {
                UserName = user.UserName,
                FisrtName = user.FisrtName,
                LastName = user.LastName,
                Books = userBooks
            };

            return View(vm);
        }
    }
}
