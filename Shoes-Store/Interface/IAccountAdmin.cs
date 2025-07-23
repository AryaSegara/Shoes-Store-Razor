using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IAccountAdmin
    {
        public bool Login(AccountAdminDTO accountAdminDTO);
        public List<RegisterDTO> GetAllUser();
        public User GetUserById(int id);
        public bool UpdateStatusUser(RegisterDTO registerDTO);
        public bool DeleteUser(int id);
        public List<SelectListItem> Users();
    }
}
