using Microsoft.AspNetCore.Mvc;

namespace ExpressWorld.API.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
