namespace Shoes_Store.Models.DTO
{
    public class CartDetailDTO
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        public IFormFile ImageFile { get; set; }
        public int TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
