using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;
using static Shoes_Store.Models.GeneralOrderStatus;
using static Shoes_Store.Models.GeneralPaymentStatus;

namespace Shoes_Store.Service
{
    public class CartService : ICart
    {
        private readonly ApplicationContext _context;

        public CartService(ApplicationContext context)
        {
            _context = context;
        }

        public CartDTO GetCart(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Product)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductSize)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return new CartDTO();
            }

            var items = cart.CartDetails.Select(cd => new CartItemDTO
            {
                ProductId = cd.ProductId,
                ProductName = cd.Product.Name,
                ImageUrl ="/images/" +  cd.Product.Image,
                Price = cd.Product.Price,
                ProductSizeId = cd.ProductSizeId,
                Size = cd.ProductSize.Size,
                Quantity = cd.Quantity
            }).ToList();

            return new CartDTO
            {
                Items = items,
                Total = items.Sum(i => i.Price * i.Quantity)
            };
        }

        public void AddToCart(int userId, int productId, int productSizeId, int quantity)
        {
            // Tambahkan pengecekan ini
            if (productSizeId <= 0)
            {
                // Beri pesan error yang lebih jelas agar mudah di-debug
                throw new ArgumentException("Product Size ID yang diterima tidak valid.", nameof(productSizeId));
            }

            var cart = _context.Carts.Include(c => c.CartDetails).FirstOrDefault(c => c.UserId == userId)
                       ?? new Cart { UserId = userId, CartDetails = new List<CartDetail>() };

            var existing = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId && cd.ProductSizeId == productSizeId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.CartDetails.Add(new CartDetail
                {
                    ProductId = productId,
                    ProductSizeId = productSizeId,
                    Quantity = quantity
                });
            }

            if (cart.Id == 0)
                _context.Carts.Add(cart);

            _context.SaveChanges();
        }

        public void UpdateQuantity(int userId, int productId, int productSizeId, int quantity)
        {
            var detail = _context.CartDetails
                .Include(cd => cd.Cart)
                .FirstOrDefault(cd => cd.Cart.UserId == userId && cd.ProductId == productId && cd.ProductSizeId == productSizeId);

            if (detail != null)
            {
                detail.Quantity = quantity;
                _context.SaveChanges();
            }
        }

        public void RemoveFromCart(int userId, int productId, int productSizeId)
        {
            var detail = _context.CartDetails
                .Include(cd => cd.Cart)
                .FirstOrDefault(cd => cd.Cart.UserId == userId && cd.ProductId == productId && cd.ProductSizeId == productSizeId);

            if (detail != null)
            {
                _context.CartDetails.Remove(detail);
                _context.SaveChanges();
            }
        }

        public (bool Success, string Message, int? OrderId) CheckoutAndCreateOrder(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Product)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.ProductSize)
                .FirstOrDefault(c => c.UserId == userId);

            var user = _context.Users
                .Include(u => u.UserSaldos)
                .FirstOrDefault(u => u.Id == userId);

            if (cart == null || user == null)
                return (false, "Cart atau user tidak ditemukan", null);

            var saldo = user.UserSaldos.OrderByDescending(s => s.Id).FirstOrDefault()?.Saldo ?? 0;
            var total = cart.CartDetails.Sum(cd => cd.Product.Price * cd.Quantity);

            if (saldo < total)
                return (false, "Saldo tidak cukup", null);

            foreach (var item in cart.CartDetails)
            {
                var size = _context.ProductSizes.FirstOrDefault(ps => ps.Id == item.ProductSizeId);
                if (size == null || size.Stock < item.Quantity)
                    return (false, $"Stok tidak cukup untuk produk {item.Product.Name} ukuran {size?.Size}", null);

                size.Stock -= item.Quantity;
            }

            var payment = new Payment
            {
                TotalAmount = total,
                PaymentStatus = GeneralPaymentStatusData.Pending,
                PaymentDate = DateTime.Now,
                PaymentMethod = "Transfer"
            };
            _context.Payments.Add(payment);

            var order = new Order
            {
                UserId = userId,
                OrderCode = $"ORD{DateTime.Now.Ticks}",
                OrderDate = DateTime.Now,
                Payment = payment,
                Status = GeneralOrderStatusData.Processing,
                OrderDetails = cart.CartDetails.Select(cd => new OrderDetail
                {
                    ProductId = cd.ProductId,
                    Quantity = cd.Quantity,
                    SelectedSize = cd.ProductSize.Size,
                    Image = cd.Product.Image,
                    PriceAtPurchase = cd.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            // Cek apakah UserSaldo sudah ada
            var existingSaldo = user.UserSaldos
                .OrderByDescending(s => s.Id)
                .FirstOrDefault();

            if (existingSaldo != null)
            {
                // Update saldo yang sudah ada
                existingSaldo.Saldo -= total;
                existingSaldo.LastUpdated = DateTime.Now;
                _context.UserSaldos.Update(existingSaldo);
            }
            else
            {
                // Kalau belum ada, buat entri baru
                _context.UserSaldos.Add(new UserSaldo
                {
                    UserId = userId,
                    Saldo = saldo - total,
                    LastUpdated = DateTime.Now
                });
            }


            _context.CartDetails.RemoveRange(cart.CartDetails);
            _context.Carts.Remove(cart);

            _context.SaveChanges();

            return (true, "Checkout berhasil", order.Id);
        }
    }
}
