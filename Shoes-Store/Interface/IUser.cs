using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IUser
    {
        Task<bool> Register(RegisterDTO registerDTO);
        Task<(bool Success, int UserId)> Login(LoginDTO loginDTO);
        public bool UpdateUserProfile(UserProfileDTO userProfileDTO);
        public User GetUserProfileeById(int id);
        public List<SelectListItem> Users();

    }
}
