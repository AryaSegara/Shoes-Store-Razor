namespace Shoes_Store.Models
{
    public class GeneralOrderStatus
    {
        public enum GeneralOrderStatusData
        {
            Canceled, //dibatalkan
            Unpaid, //Belum Bayar
            Processing, // Dikemas
            Shipped, // Dikirim
            Dilivered // Selesai
        }
    }
}
