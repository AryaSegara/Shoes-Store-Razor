using Shoes_Store.Models.DB;

namespace Shoes_Store.Models.DTO
{
    public class PaymentViewDTO
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public int TotalAmount { get; set; }
        public decimal saldo { get; set; }
    }
}
