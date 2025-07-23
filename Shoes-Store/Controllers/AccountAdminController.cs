using Microsoft.AspNetCore.Mvc;

namespace Shoes_Store.Controllers
{
    public class AccountAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
