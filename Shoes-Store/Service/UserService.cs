using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;
using static Shoes_Store.Models.GeneralStatus;

namespace Shoes_Store.Service
{
    public class UserService : IUser
    {
        private readonly ApplicationContext _context; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(ApplicationContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Register(RegisterDTO registerDTO)
        {
            try
            {
                if(await _context.Users.AnyAsync(u => u.Username == registerDTO.Username || u.Email == registerDTO.Email))
                {
                    return false;
                }

                var passwordHash = HashPassword(registerDTO.Password);

                var user = new User
                {
                    Name = "",
                    Username = registerDTO.Username,
                    Email = registerDTO.Email,
                    Password = passwordHash,
                    Address = "",
                    PhoneNumber = "",
                    Image = "",
                    DateOfBirth = "",
                    CreatedAt = DateTime.Now,
                    UserStatus = GeneralStatus.GeneralStatusData.Published,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                //simpan session
                _httpContextAccessor.HttpContext?.Session.SetInt32("UserId", user.Id);
                _httpContextAccessor.HttpContext?.Session.SetString("Name", user.Name);

                // tambah saldo user berdasarkan user.Id
                _context.UserSaldos.Add(new UserSaldo { UserId = user.Id });
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {

                throw new Exception("Terjadi Kesalahan saat Register", ex);
            }
        }

        public async Task<(bool Success, int UserId)> Login (LoginDTO loginDTO)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Username == loginDTO.Username &&
                    u.UserStatus == GeneralStatus.GeneralStatusData.Published);

                if (user == null)
                {
                    return (false, 0);
                }

                if (!VerifyPassword(loginDTO.Password, user.Password))
                {
                    return (false,0);
                }

                return (true,user.Id);

            }
            catch (Exception ex)
            {

                throw new Exception("Terjadi Kesalahan saat login", ex);
            }

        }

        public bool UpdateUserProfile(UserProfileDTO userProfileDTO)
        {
            try
            {
                var user = _context.Users.Find(userProfileDTO.Id);
                if(user == null)
                {
                    return false;
                }

                //update user properties
                user.Name = userProfileDTO.Name;
                user.Username = userProfileDTO.Username;
                user.Email = userProfileDTO.Email;
                user.PhoneNumber = userProfileDTO.PhoneNumber;
                user.Address = userProfileDTO.Address;
                user.DateOfBirth = userProfileDTO.DateOfBirth;
                user.UpdatedAt = DateTime.Now;

                //proses upload image baru jika ada
                if(userProfileDTO.ImageFile != null && userProfileDTO.ImageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(userProfileDTO.ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                        throw new InvalidOperationException("File format not supported. Only .jpg, .jpeg, .png, .gif allowed.");

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    Directory.CreateDirectory(folderPath); // aman jika folder sudah ada

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        userProfileDTO.ImageFile.CopyTo(stream);
                    }

                    user.Image = fileName;
                }

                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {

                throw new Exception("Terjadi Kesalahan saat Update", ex);
            }
        }

        public User GetUserProfileeById(int id)
        {
            var data = _context.Users.Where(x => x.Id == id && x.UserStatus != GeneralStatusData.delete).FirstOrDefault();
            if (data == null)
            {
                return new User();
            }

            return data;
        }

        private string HashPassword(string pin)
        {
            // Implementasi hashing password (gunakan BCrypt atau metode secure lainnya)
            return BCrypt.Net.BCrypt.HashPassword(pin);
        }


        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }


}
