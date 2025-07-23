using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Service
{
    public class UserSaldoService : IUserSaldo
    {
        private readonly ApplicationContext _context;

        public UserSaldoService(ApplicationContext context)
        {
            _context = context;
        }

        public List<UserSaldoDTO> GetListUserSaldo()
        {
            var data = _context.UserSaldos.Include(y => y.User).Select(x => new UserSaldoDTO
            {
                Id = x.Id,
                UserName = x.User.Name,
                Saldo = x.Saldo,

            }).ToList();
            return data;

        }

        public UserSaldo GetUserSaldoById(int userId)
        {
            var data = _context.UserSaldos.FirstOrDefault(s => s.UserId == userId);
            if (data == null)
            {
                return new UserSaldo();
            }
            return data;
        }

        public bool AddUserSaldo(UserSaldoDTO userSaldoDTO)
        {
            try
            {
                var existingSaldo = _context.UserSaldos.FirstOrDefault(s => s.UserId == userSaldoDTO.UserId);

                if (existingSaldo == null)
                {
                    // Validasi saldo tidak boleh negatif untuk record baru
                    if (userSaldoDTO.Saldo < 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Saldo awal tidak boleh negatif");
                        return false;
                    }

                    var newSaldo = new UserSaldo
                    {
                        UserId = userSaldoDTO.UserId,
                        Saldo = userSaldoDTO.Saldo,
                        LastUpdated = DateTime.Now
                    };
                    _context.UserSaldos.Add(newSaldo);
                }
                else
                {
                    // Validasi saldo akhir tidak boleh negatif
                    if (existingSaldo.Saldo + userSaldoDTO.Saldo < 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Saldo tidak mencukupi");
                        return false;
                    }

                    existingSaldo.Saldo += userSaldoDTO.Saldo;
                    existingSaldo.LastUpdated = DateTime.Now;
                    _context.UserSaldos.Update(existingSaldo);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool ReduceUserSaldo(int userId, decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("Jumlah pengurangan harus positif");
                    return false;
                }

                var existingSaldo = _context.UserSaldos.FirstOrDefault(s => s.UserId == userId);
                if (existingSaldo == null)
                {
                    System.Diagnostics.Debug.WriteLine("Saldo user tidak ditemukan");
                    return false;
                }

                if (existingSaldo.Saldo < amount)
                {
                    System.Diagnostics.Debug.WriteLine("Saldo tidak mencukupi");
                    return false;
                }

                existingSaldo.Saldo -= (int)amount;
                existingSaldo.LastUpdated = DateTime.Now;
                _context.UserSaldos.Update(existingSaldo);
                _context.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"Saldo berhasil dikurangi. Saldo sekarang: {existingSaldo.Saldo}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error mengurangi saldo: {ex.Message}");
                return false;
            }
        }

    }
}
