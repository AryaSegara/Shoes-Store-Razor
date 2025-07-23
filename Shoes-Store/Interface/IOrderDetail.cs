using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IOrderDetail
    {
        public List<OrderDetailDTO> GetListOrderDetail();
        public OrderDetail GetOrderDetailById(int id);
        public bool DeleteOrderDetail(int id);

    }
}
