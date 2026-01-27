using Microsoft.AspNetCore.Mvc;

namespace MovieLists.Controllers
{
    public class MovieViewController : Controller
    {
        [Route("Movie/Detail/{id:int}")]
        public IActionResult Detail(int id)
        {
            return View("~/Views/Movie/Detail.cshtml");
        }
    }
}