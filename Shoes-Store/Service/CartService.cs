using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Service
{
    public class CartService : ICart
    {
        private readonly ApplicationContext _context;

        public CartService(ApplicationContext context)
        {
            _context = context;
        }

        public void AddToCart(int userId, int productId)
        {
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartDetails = new List<CartDetail>() };
                _context.Carts.Add(cart);
                _context.SaveChanges(); // save to get cart ID
            }

            var cartDetail = _context.CartDetails
                .FirstOrDefault(cd => cd.CartId == cart.Id && cd.ProductId == productId);

            if (cartDetail != null)
            {
                cartDetail.Quantity++;
            }
            else
            {
                _context.CartDetails.Add(new CartDetail
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                });
            }

            _context.SaveChanges();
        }

        public CartDTO GetCart(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return new CartDTO { Items = new List<CartItemDTO>() };
            }

            var items = cart.CartDetails.Select(cd => new CartItemDTO
            {
                ProductId = cd.ProductId,
                ProductName = cd.Product.Name,
                ImageUrl = "/images/" +  cd.Product.Image,
                Price = cd.Product.Price,
                Quantity = cd.Quantity
            }).ToList();

            return new CartDTO { Items = items };
        }

        public void UpdateQuantity(int userId, int productId, int quantity)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return;

            var item = _context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.Id && cd.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                _context.SaveChanges();
            }
        }

        public void RemoveItem(int userId, int productId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return;

            var item = _context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.Id && cd.ProductId == productId);
            if (item != null)
            {
                _context.CartDetails.Remove(item);
                _context.SaveChanges();
            }
        }

        //public void Checkout(int userId)
        //{
        //    var cart = _context.Carts
        //        .Include(c => c.CartDetails)
        //        .ThenInclude(cd => cd.Product)
        //        .FirstOrDefault(c => c.UserId == userId);

        //    if (cart == null || !cart.CartDetails.Any()) return;

        //    var order = new Order
        //    {
        //        UserId = userId,
        //        OrderDate = DateTime.Now,
        //        PaymentId = 1, // contoh default payment
        //        OrderDetails = new List<OrderDetail>()
        //    };

        //    foreach (var item in cart.CartDetails)
        //    {
        //        order.OrderDetails.Add(new OrderDetail
        //        {
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity,
        //            //Price = item.Product.Price
        //        });
        //    }

        //    _context.Orders.Add(order);

        //    // bersihkan keranjang
        //    _context.CartDetails.RemoveRange(cart.CartDetails);

        //    _context.SaveChanges();
        //}
    }
}
