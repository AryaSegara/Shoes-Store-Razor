using static Shoes_Store.Models.GeneralStatus;

namespace Shoes_Store.Models.DB
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        //public int Stok { get; set; }
        public int CategoryId { get; set; } //foreign key category
        public Category Category { get; set; }
        public string Image { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public GeneralStatusData ProductStatus { get; set; }

        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
