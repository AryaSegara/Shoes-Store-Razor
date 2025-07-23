using System.ComponentModel.DataAnnotations.Schema;
using static Shoes_Store.Models.GeneralPaymentStatus;

namespace Shoes_Store.Models.DB
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } // Misalnya: "Credit Card", "PayPal", "Bank Transfer"
        public GeneralPaymentStatusData PaymentStatus { get; set; } // Contoh: "Pending", "Completed", "Failed"

        [Column(TypeName = "decimal(18,2)")]
        public int TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? BankName { get; internal set; }
        public string? ProofImage { get; internal set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
