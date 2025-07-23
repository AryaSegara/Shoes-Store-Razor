using System.ComponentModel.DataAnnotations.Schema;

namespace Shoes_Store.Models.DB
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public string SelectedSize { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public int PriceAtPurchase { get; set; } // Harga saat pembelian
    }
}
