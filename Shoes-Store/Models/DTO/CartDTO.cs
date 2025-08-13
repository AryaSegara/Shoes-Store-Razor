namespace Shoes_Store.Models.DTO
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal Total { get; set; }
    }

}
