namespace Shoes_Store.Models.DTO
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Foreign key ke Order
        public string OrderCode { get; set; }
        public int ProductId { get; set; } // Foreign key ke Product
        public string ProductName { get; set; }
        public IFormFile ImageFile { get; set; } //POST and PUT

        public string ImageUrl { get; set; } // GET
        public int Quantity { get; set; }
        public string SelectedSize { get; set; }
        public int PriceAtPurchase { get; set; } // Harga saat pembelian
    }
}
