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

        [HttpGet]
        public IActionResult Index()
        {
            var cart = _cart.GetCart(GetCurrentUserId());
            return View(cart);
        }

        [HttpPost]
        public IActionResult Add(int productId, int productSizeId, int quantity)
        {
            _cart.AddToCart(GetCurrentUserId(), productId, productSizeId, quantity);
            return RedirectToAction("ListProduct","HomeUser");
        }

        [HttpPost]
        public IActionResult Update(int productId, int productSizeId, int quantity)
        {
            _cart.UpdateQuantity(GetCurrentUserId(), productId, productSizeId, quantity);
            return RedirectToAction("Index","Cart");
        }

        [HttpPost]
        public IActionResult Remove(int productId, int productSizeId)
        {
            _cart.RemoveFromCart(GetCurrentUserId(), productId, productSizeId);
            return RedirectToAction("Index","Cart");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            var result = _cart.CheckoutAndCreateOrder(GetCurrentUserId());

            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                return RedirectToAction("Index", "Cart");
            }

            return RedirectToAction("UploadProof", "OrderUser", new { orderId = result.OrderId });
        }

    }
}
