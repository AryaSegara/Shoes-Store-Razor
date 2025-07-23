using System.ComponentModel.DataAnnotations.Schema;

namespace Shoes_Store.Models.DTO
{
    public class UserSaldoDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public int Saldo { get; set; } = 0;
        public int CurrentSaldo { get; set; }
    }
}
