using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Service;

namespace Shoes_Store.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICart _cart;

        public CartController(ICart cart)
        {
            _cart = cart;
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            int userId = GetCurrentUserId(); // ganti sesuai sistem login kamu
            _cart.AddToCart(userId, productId);
            return RedirectToAction("ListProduct","HomeUser");
        }

        public IActionResult Index()
        {
            int userId = GetCurrentUserId(); // ganti sesuai login kamu
            var cart = _cart.GetCart(userId);
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            int userId = GetCurrentUserId();
            _cart.UpdateQuantity(userId, productId, quantity);
            return RedirectToAction("Index","Cart");
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            int userId = GetCurrentUserId();
            _cart.RemoveItem(userId, productId);
            return RedirectToAction("Index","Cart");
        }

        //[HttpPost]
        //public IActionResult Checkout()
        //{
        //    int userId = GetCurrentUserId();
        //    _cart.Checkout(userId);
        //    TempData["Message"] = "Checkout berhasil!";
        //    return RedirectToAction("Index");
        //}
    }
}
