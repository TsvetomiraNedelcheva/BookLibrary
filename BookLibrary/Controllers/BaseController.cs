using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
       
    }
}
