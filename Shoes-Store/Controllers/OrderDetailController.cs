using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;

namespace Shoes_Store.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetail _orderDetail;

        public OrderDetailController(IOrderDetail orderDetail)
        {
            _orderDetail = orderDetail;
        }

        //Get ListOrderDetailDiAdmin
        public IActionResult Index()
        {
            var data = _orderDetail.GetListOrderDetail();
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _orderDetail.DeleteOrderDetail(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus.");
        }
    }
}
