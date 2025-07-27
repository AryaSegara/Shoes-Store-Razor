using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        private readonly ApplicationContext _contex;

        public LoginUser(IUser user, ApplicationContext context)
        {
            _user = user;
            _contex = context;
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
                var user = await _contex.Users.FindAsync(userId);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim("UserId", user.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Konfigurasi properti autentikasi berdasarkan pilihan "Remember Me"
                var authProperties = new AuthenticationProperties();

                if (loginDTO.RememberMe)
                {

                    authProperties.IsPersistent = true;
                    authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7);

                    Console.WriteLine("Remember Me diaktifkan. Cookie akan bertahan 7 hari.");
                }
                else
                {

                    authProperties.IsPersistent = false;
                    authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120);

                    Console.WriteLine("Remember Me tidak diaktifkan. Cookie akan bertahan selama sesi browser.");
                }


                //meng encrpty
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties
                );


                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name);

                return RedirectToAction("Index", "Home");

            }

            TempData["ErrorMessage"] = "Username atau Password salah!";
            return View(loginDTO);
        

        }
    }
}
