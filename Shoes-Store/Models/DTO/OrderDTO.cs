using static Shoes_Store.Models.GeneralOrderStatus;

namespace Shoes_Store.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PaymentId { get; set; }

        public DateTime OrderDate { get; set; }
        public GeneralOrderStatusData Status { get; set; }
    }
}
