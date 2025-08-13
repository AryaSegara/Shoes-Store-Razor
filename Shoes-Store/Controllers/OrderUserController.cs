using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Service;

namespace Shoes_Store.Controllers
{
    public class OrderUserController : Controller
    {
        private readonly IOrder _order;
        private readonly IWebHostEnvironment _env;

        public OrderUserController(IOrder order, IWebHostEnvironment env)
        {
            _order = order;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadProof(int orderId)
        {
            var order = _order.GetOrderWithDetails(orderId);
            if (order == null) return NotFound();

            ViewBag.Saldo = order.User.UserSaldos.OrderByDescending(s => s.Id).FirstOrDefault()?.Saldo ?? 0;

            return View(order);
        }

        [HttpPost]
        public IActionResult UploadProof(int orderId, IFormFile proofImage, string bankName)
        {
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!_order.UploadPaymentProof(orderId, proofImage, bankName, uploadsPath))
            {
                TempData["Message"] = "Gagal mengupload bukti pembayaran.";
                return RedirectToAction("UploadProof", new { orderId });
            }

            TempData["Message"] = "Bukti pembayaran berhasil dikirim.";
            return RedirectToAction("ListProduct","HomeUser");
        }
    }
}
