using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface ICart
    {
        void AddToCart(int userId, int productId);
        CartDTO GetCart(int userId);
        void UpdateQuantity(int userId, int productId, int quantity);
        void RemoveItem(int userId, int productId);
        //void Checkout(int userId);
    }

}
