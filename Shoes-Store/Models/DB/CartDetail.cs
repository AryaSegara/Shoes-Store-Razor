namespace Shoes_Store.Models.DB
{
    public class CartDetail
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }    
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }
        public int Quantity { get; set; }
    }
}
