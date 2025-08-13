using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;

namespace Shoes_Store.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPayment _payment;

        public PaymentController(IPayment payment)
        {
            _payment = payment;
        }
        public IActionResult Index()
        {
            var data = _payment.GetListPayment();
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _payment.DeletePayment(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus.");
        }
    }
}
