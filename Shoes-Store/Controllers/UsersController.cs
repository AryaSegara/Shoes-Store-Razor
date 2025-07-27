using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAccountAdmin _accountAdmin;

        public UsersController(IAccountAdmin accountAdmin)
        {
            _accountAdmin = accountAdmin;
        }
        public IActionResult Index()
        {
            var data = _accountAdmin.GetAllUser();
            return View(data);
        }

        public IActionResult Update(int id)
        {
            var data = _accountAdmin.GetUserById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(RegisterDTO registerDTO)
        {
            var data = _accountAdmin.UpdateStatusUser(registerDTO);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _accountAdmin.DeleteUser(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus User.");
        }
    }
}
