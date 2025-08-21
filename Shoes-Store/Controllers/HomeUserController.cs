using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class HomeUserController : BaseController
    {
        private readonly IUser _user;
        private readonly IUserSaldo _userSaldo;
        private readonly IProduct _product;
        private readonly ApplicationContext _context;

        public HomeUserController(IUser user, IUserSaldo userSaldo, IProduct product, ApplicationContext context)
        {
            _user = user;
            _userSaldo = userSaldo;
            _product = product;
            _context = context;
        }


        public IActionResult ListProduct()
        {
            var data = _product.GetListProduct();

            return View(data);
        }

        public IActionResult TopUpSaldo()
        {
            var userId = GetCurrentUserId();

            var saldo = _context.UserSaldos.FirstOrDefault(u => u.UserId ==  userId)?.Saldo ?? 0;

            ViewBag.CurrentSaldo = saldo;
            return View(); 
        }

        //GET
        public IActionResult MyProfile()
        {

            int userId = GetCurrentUserId();
            var user = _user.GetUserProfileeById(userId);

            var baseUrl = "/images/";
            // Map user data to DTO for the view
            var userProfileDTO = new UserProfileDTO
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                ImageUrl = baseUrl + user.Image
            };
            var saldo = _context.UserSaldos.FirstOrDefault(u => u.UserId == userId)?.Saldo ?? 0;

            ViewBag.CurrentSaldo = saldo;


            return View(userProfileDTO);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> MyProfile(UserProfileDTO userProfileDTO)
        {
            int userId = GetCurrentUserId();
            userProfileDTO.Id = userId;

            var currentUser = _user.GetUserProfileeById(userId);
            if(currentUser == null)
            {
                return View(userProfileDTO);
            }

            // Save changes to database
            var success = _user.UpdateUserProfile(userProfileDTO);
            System.Diagnostics.Debug.WriteLine($"[TopUp POST] UserId dari session: {success}");

            if (success)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("MyProfile", "HomeUser");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update profile. Please try again.";
                return View(userProfileDTO);  // Tambahkan return di sini agar tidak lanjut ke kode berikutnya
            }
        }

        [HttpPost]
        public IActionResult TopUpSaldo(int topupAmount, UserSaldoDTO userSaldoDTO)
        {

            var userId = GetCurrentUserId();
            userSaldoDTO.Id = userId;

            if (topupAmount < 10000)
            {
                TempData["Error"] = "Minimal top up Rp 10.000";
                return RedirectToAction("TopUpSaldo","HomeUser");
            }

            var result = _userSaldo.AddUserSaldo(new UserSaldoDTO
            {
                UserId = userId,
                Saldo = topupAmount
            });

            if (result)
            {
                TempData["Success"] = $"Top up sebesar Rp {topupAmount:N0} berhasil!";
            }
            else
            {
                TempData["Error"] = $"Top up gagal! Pastikan saldo tidak negatif.";
            }

            return RedirectToAction("MyProfile", "HomeUser", userSaldoDTO);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("ListProductHome", "Home");
        }
    }
}
