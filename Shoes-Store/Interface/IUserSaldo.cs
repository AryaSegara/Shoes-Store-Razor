using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IUserSaldo
    {
        public List<UserSaldoDTO> GetListUserSaldo();
        public UserSaldo GetUserSaldoById(int userId);
        public bool AddUserSaldo(UserSaldoDTO userSaldoDTO);
        public bool ReduceUserSaldo(int userId, decimal amount);

        public bool DeleteUserSaldo(int id);
    }
}
