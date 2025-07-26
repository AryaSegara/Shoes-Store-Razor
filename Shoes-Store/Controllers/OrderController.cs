using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrder _order;
        private readonly IUser _user;

        public OrderController(IOrder order, IUser user)
        {
            _order = order;
            _user = user;
        }

        //GET CONTROLLER
        public IActionResult Index()
        {
            var data = _order.GetListOrderan();
            return View(data);
        }


        public IActionResult Update(int id)
        {
            ViewBag.User = _user.Users();
            var data = _order.GetOrderanById(id);
            return View(data);
        }


        //UPDATE
        [HttpPost]
        public IActionResult Update(OrderDTO orderanDTO)
        {
            if (orderanDTO.Id == 0)
            {
                var data = _order.AddOrderan(orderanDTO);
                if (data)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                var data = _order.EditOrderan(orderanDTO);
                if (data)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View();

        }


        //DELETE
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _order.DeleteOrderan(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus produk.");
        }
    }
}
