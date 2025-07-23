using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Service
{
    public class OrderDetailService : IOrderDetail
    {
        private readonly ApplicationContext _context;

        public OrderDetailService(ApplicationContext context)
        {
            _context = context;
        }

        public List<OrderDetailDTO> GetListOrderDetail()
        {
            var baseUrl = "/images/";

            var data = _context.OrderDetails.Include(a => a.Order).Include(a => a.Product).Select(x => new OrderDetailDTO
            {
                Id = x.Id,
                OrderCode = x.Order.OrderCode,
                ProductName = x.Product.Name,
                ImageUrl = baseUrl + x.Image,
                Quantity = x.Quantity,
                SelectedSize = x.SelectedSize,
                PriceAtPurchase = x.PriceAtPurchase,


            }).ToList();
            return data;

        }

        public OrderDetail GetOrderDetailById(int id)
        {
            var data = _context.OrderDetails.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return new OrderDetail();
            }
            return data;
        }

        public bool DeleteOrderDetail(int id)
        {
            var data = _context.OrderDetails.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return false;
            }

            _context.OrderDetails.Remove(data);
            _context.SaveChanges();
            return true;
        }
    }
}
