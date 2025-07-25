using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class LoginAdminController : Controller
    {
        private readonly IAccountAdmin _accountAdmin;

        public LoginAdminController(IAccountAdmin accountAdmin)
        {
            _accountAdmin = accountAdmin;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login(AccountAdminDTO accountAdminDTO)
        {
            try
            {
                var isValid = _accountAdmin.Login(accountAdminDTO);
                if (!isValid)
                {
                    ViewBag.ErrorMessage = "Nama atau PIN salah.";
                    return View("Index"); // kembali ke halaman login dengan pesan error
                }

                // (Opsional) Simpan data ke session, cookie, dsb
                //HttpContext.Session.SetString("AdminName", accountAdminDTO.Name); // butuh konfigurasi session sebelumnya

                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "DashboardAdmin");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Terjadi kesalahan: " + ex.Message;
                return View("Index");
            }
        }

    }
}
