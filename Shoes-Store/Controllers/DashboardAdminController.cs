using Microsoft.AspNetCore.Mvc;

namespace Shoes_Store.Controllers
{
    public class DashboardAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
