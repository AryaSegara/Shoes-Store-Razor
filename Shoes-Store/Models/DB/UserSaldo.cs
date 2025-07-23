using System.ComponentModel.DataAnnotations.Schema;

namespace Shoes_Store.Models.DB
{
    public class UserSaldo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public int Saldo { get; set; } = 0;
        public User User { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
