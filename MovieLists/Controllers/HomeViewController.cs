using Microsoft.AspNetCore.Mvc;

namespace MovieLists.Controllers
{
    public class HomeViewController : Controller
    {
        [Route("")]
        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }
}