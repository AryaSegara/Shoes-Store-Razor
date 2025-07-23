using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IOrder
    {
        public List<OrderDTO> GetListOrderan();
        public Order GetOrderanById(int id);
        public bool EditOrderan(OrderDTO orderanDTO);
        public bool DeleteOrderan(int id);
        public bool AddOrderan(OrderDTO orderanDTO);
        public List<SelectListItem> Orders();
        public int CreateNewOrder(int userId, int productId, int productSizeId, int quantity);

    }
}
