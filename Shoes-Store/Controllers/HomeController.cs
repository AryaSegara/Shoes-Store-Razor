using Microsoft.AspNetCore.Mvc;

namespace Shoes_Store.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
