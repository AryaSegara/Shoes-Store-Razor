using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface ICart
    {
        CartDTO GetCart(int userId);
        void AddToCart(int userId, int productId, int productSizeId, int quantity);
        void UpdateQuantity(int userId, int productId, int productSizeId, int quantity);
        void RemoveFromCart(int userId, int productId, int productSizeId);
        public (bool Success, string Message, int? OrderId) CheckoutAndCreateOrder(int userId);
    }
}
