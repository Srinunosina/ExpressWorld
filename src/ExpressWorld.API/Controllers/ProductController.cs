using Microsoft.AspNetCore.Mvc;

namespace ExpressWorld.API.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
