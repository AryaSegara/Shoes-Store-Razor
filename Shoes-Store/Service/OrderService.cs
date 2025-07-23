using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;
using static Shoes_Store.Models.GeneralOrderStatus;

namespace Shoes_Store.Service
{
    public class OrderService : IOrder
    {
        private readonly ApplicationContext _context;

        public OrderService(ApplicationContext context)
        {
            _context = context;
        }

        public List<OrderDTO> GetListOrderan()
        {
            var data = _context.Orders.Include(a => a.User).Select(x => new OrderDTO
            {
                Id = x.Id,
                UserName = x.User.Name,
                OrderCode = x.OrderCode,
                Status = x.Status,


            }).ToList();
            return data;

        }

        public Order GetOrderanById(int id)
        {
            var data = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return new Order();
            }
            return data;
        }

        // admin
        public bool EditOrderan(OrderDTO orderanDTO)
        {
            var data = _context.Orders.FirstOrDefault(x => x.Id == orderanDTO.Id);
            if (data == null)
            {
                return false;
            }

            data.UserId = orderanDTO.UserId;
            data.OrderCode = orderanDTO.OrderCode;
            data.OrderDate = orderanDTO.OrderDate;
            data.Status = orderanDTO.Status;

            _context.Orders.Update(data);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteOrderan(int id)
        {
            var data = _context.Orders.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return false;
            }

            _context.Orders.Remove(data);
            _context.SaveChanges();
            return true;
        }

        // ini pada saat si user melakukan orderan
        public bool AddOrderan(OrderDTO orderanDTO)
        {

            var data = new Order();

            data.UserId = orderanDTO.UserId;
            data.OrderCode = orderanDTO.OrderCode;
            data.Status = orderanDTO.Status;
            data.OrderDate = DateTime.Now;

            _context.Orders.Add(data);
            _context.SaveChanges();
            return true;

        }

        public List<SelectListItem> Orders()
        {
            var datas = _context.Orders
                .Select(x => new SelectListItem
                {
                    Text = x.OrderCode,
                    Value = x.Id.ToString()
                }).ToList();

            return datas;
        }

        // chekout
        public int CreateNewOrder(int userId, int productId, int productSizeId, int quantity)
        {
            try
            {
                // Generate order code
                string orderCode = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";


                var product = _context.Products.Find(productId);
                var productSize = _context.ProductSizes
                    .Include(ps => ps.Product)
                    .FirstOrDefault(ps => ps.Id == productSizeId);

                if (product == null || productSize == null)
                {
                    throw new Exception("Produk atau ukuran produk tidak ditemukan");
                }


                if (productSize.Stock < quantity)
                {

                    throw new Exception($"Stok tidak mencukupi untuk ukuran {productSize.Size}. Stok tersedia: {productSize.Stock}");
                }

                // Create new order
                var order = new Order
                {
                    UserId = userId,
                    OrderCode = orderCode,
                    OrderDate = DateTime.Now,
                    Status = GeneralOrderStatusData.Unpaid
                };

                _context.Orders.Add(order);
                _context.SaveChanges(); // Simpan untuk mendapatkan ID order

                // Create order detail
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    SelectedSize = productSize.Size,
                    PriceAtPurchase = product.Price,
                    Image = product.Image
                };

                _context.OrderDetails.Add(orderDetail);

                // Update stock
                productSize.Stock -= quantity;
                _context.ProductSizes.Update(productSize);

                _context.SaveChanges();

                return order.Id;
            }
            catch (Exception ex)
            {
                // Log error jika diperlukan
                throw new Exception("Gagal membuat order: " + ex.Message);
            }
        }
    }
}
