namespace BookLibrary.Areas.Administration.Controller
{
    using BookLibrary.Core.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = UserConstantts.Admin)]
    [Area("Administration")]
    public class BaseController : Controller
    {
       
    }
}
