using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Areas.Administration.Controller
{
    public class BookController : BaseController
    {
        public IActionResult Add()
        {
            return RedirectToAction("Add", "Book");
        }
    }
}
