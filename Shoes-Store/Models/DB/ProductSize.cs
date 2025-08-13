namespace Shoes_Store.Models.DB
{
    public class ProductSize
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Size { get; set; }
        public int Stock { get; set; }

        public ICollection<CartDetail> cartDetails { get; set; }
    }
}
