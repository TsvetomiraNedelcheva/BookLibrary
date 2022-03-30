using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Areas.Administration.Controller
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
