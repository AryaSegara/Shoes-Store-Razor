namespace Shoes_Store.Models.DB
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<CartDetail> CartDetails = new List<CartDetail>();
    }
}
