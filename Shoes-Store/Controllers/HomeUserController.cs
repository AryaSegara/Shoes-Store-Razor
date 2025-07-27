using Microsoft.AspNetCore.Mvc;

namespace Shoes_Store.Controllers
{
    public class HomeUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
