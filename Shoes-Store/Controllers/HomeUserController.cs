using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class HomeUserController : BaseController
    {
        private readonly IUser _user;
        private readonly IUserSaldo _userSaldo;
        private readonly IProduct _product;

        public HomeUserController(IUser user, IUserSaldo userSaldo, IProduct product)
        {
            _user = user;
            _userSaldo = userSaldo;
            _product = product;
        }


        public IActionResult ListProduct()
        {
            var data = _product.GetListProduct();

            return View(data);
        }

        public IActionResult TopUpSaldo()
        {
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

            return View(userProfileDTO);
        }


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

     

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("ListProductHome", "Home");
        }
    }
}
