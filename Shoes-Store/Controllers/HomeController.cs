using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;

namespace Shoes_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProduct _product;

        public HomeController(IProduct product)
        {
            _product = product;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ListProductHome()
        {
            var data = _product.GetListProduct();

            return View(data);
        }
    }
}
