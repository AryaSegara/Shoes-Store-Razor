using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;
using static Shoes_Store.Models.GeneralStatus;

namespace Shoes_Store.Service
{
    public class AccountAdminService : IAccountAdmin
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountAdminService (ApplicationContext contexts, IHttpContextAccessor contextAccessor)
        {
            _context = contexts;
            _contextAccessor = contextAccessor;
        }

        public bool Login(AccountAdminDTO accountAdminDTO)
        {
            try
            {
                // cari admin berdasarkan name
                var admin = _context.Admins
                    .FirstOrDefault(a => a.Name ==  accountAdminDTO.Name);

                if(admin == null)
                {
                    return false;
                }

                if(!VerifyPassword(accountAdminDTO.Pin, admin.Pin))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"[Login Asy")
                throw new Exception("Terjadi Kesalahan saat login", ex);
            }
        }

        public List<RegisterDTO> GetAllUser()
        {
            var data = _context.Users.Where(u => u.UserStatus != GeneralStatusData.delete).Select(u => new RegisterDTO
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                Password = "*******",
                ConfirmPassword = "*******",
                UserStatus = u.UserStatus
            }).ToList();

            return data;
        }

        public User GetUserById(int id)
        {
            var data = _context.Users.Where(u => u.Id == id && u.UserStatus != GeneralStatusData.delete).FirstOrDefault();
            if(data == null)
            {
                return new User();
            }

            return data;
           
        }

        public bool UpdateStatusUser(RegisterDTO registerDTO)
        {
            var data = _context.Users.FirstOrDefault(u => u.Id == registerDTO.Id);
            if(data == null)
            {
                return false;
            }

            data.CreatedAt = DateTime.Now;
            data.UserStatus = registerDTO.UserStatus;

            _context.Users.Update(data);
            _context.SaveChanges();

            return true;

        }

        public bool DeleteUser(int id)
        {
            var data = _context.Users.FirstOrDefault(u => u.Id == id);
            if(data == null)
            {
                return false;
            }

            data.UserStatus = GeneralStatusData.delete;
            _context.SaveChanges();
            return true;
        }

        public List<SelectListItem> Users()
        {
            var data = _context.Users
                .Where(u => u.UserStatus == GeneralStatusData.delete)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            return data;
        }


        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}
