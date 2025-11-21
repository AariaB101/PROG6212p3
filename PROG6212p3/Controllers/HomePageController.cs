using Microsoft.AspNetCore.Mvc;

namespace PROG6212p3.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
