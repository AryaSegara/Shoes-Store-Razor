namespace Shoes_Store.Models.DTO
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; }
        public decimal TotalPrice => Items?.Sum(i => i.Price * i.Quantity) ?? 0;
    }

}
