using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DTO;
using System.Security.Claims;


namespace Shoes_Store.Controllers
{
    public class LoginUser : Controller
    {
        private readonly IUser _user;

        public LoginUser(IUser user)
        {
            _user = user;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Password dan Konfirmasi password harus sama";

                return View(registerDTO);
            }

            try
            {
                var result = await _user.Register(registerDTO);
                if (result)
                {
                    TempData["SuccessMessage"] = "Registrasi berhasil! Silakan login.";
                    Console.WriteLine("Redirect ke login!");
                    return RedirectToAction("Index", "LoginUser");
                }

                TempData["ErrorMessage"] = "Gagal mendaftarkan user. Silakan coba lagi.";
                return View(registerDTO);
            }
            catch (Exception ex)
            {
                // Log exception untuk debugging
                Console.WriteLine($"Error registrasi: {ex.Message}");
                // Log juga inner exception jika ada
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                TempData["ErrorMessage"] = $"Terjadi kesalahan saat mendaftarkan user: {ex.Message}";
                return View(registerDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO loginDTO)
        {
            var (success, userId) = await _user.Login(loginDTO);

            if(success)
            {
                return RedirectToAction("ListProduct", "HomeUser");

            }

            TempData["ErrorMessage"] = "Username atau Password salah!";
            return View(loginDTO);
        

        }
    }
}
