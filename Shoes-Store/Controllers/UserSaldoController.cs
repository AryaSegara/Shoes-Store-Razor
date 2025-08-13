using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;

namespace Shoes_Store.Controllers
{
    public class UserSaldoController : Controller
    {
        private readonly IUserSaldo _userSaldo;

        public UserSaldoController(IUserSaldo userSaldo)
        {
            _userSaldo = userSaldo;
        }
        public IActionResult Index()
        {
            var data = _userSaldo.GetListUserSaldo();
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _userSaldo.DeleteUserSaldo(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus produk.");
        }
    }
}
