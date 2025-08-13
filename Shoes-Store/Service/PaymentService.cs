using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;
using static Shoes_Store.Models.GeneralOrderStatus;
using static Shoes_Store.Models.GeneralPaymentStatus;

namespace Shoes_Store.Service
{
    public class PaymentService : IPayment
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PaymentService(ApplicationContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<PaymentDTO> GetListPayment()
        {
            var data = _context.Payments.Include(a => a.Orders).Select(x => new PaymentDTO
            {
                Id = x.Id,
                OrderCode = x.Orders.Select(o => o.OrderCode).ToList(),
                PaymentMethod = x.PaymentMethod,
                TotalAmount = x.TotalAmount,
                PaymentStatus = x.PaymentStatus

            }).ToList();

            return data;
        }

        public Payment GetPaymentlById(int id)
        {
            var data = _context.Payments.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return new Payment();
            }
            return data;
        }

        public bool DeletePayment(int id)
        {
            var data = _context.Payments.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return false;
            }

            _context.Payments.Remove(data);
            _context.SaveChanges();
            return true;
        }


        //public async Task<bool> ConfirmPayment(int orderId, string paymentMethod, string selectedBank, string proofImage)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var order = await _context.Orders
        //            .Include(o => o.OrderDetails)
        //            .FirstOrDefaultAsync(o => o.Id == orderId);

        //        if (order == null)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Order tidak ditemukan");
        //            return false;
        //        }

        //        var totalAmount = order.OrderDetails.Sum(od => od.PriceAtPurchase * od.Quantity);
        //        System.Diagnostics.Debug.WriteLine($"Total pembayaran: {totalAmount}");

        //        // Dapatkan saldo user
        //        var userSaldo = await _context.UserSaldos
        //            .FirstOrDefaultAsync(s => s.UserId == order.UserId);

        //        if (userSaldo == null)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Saldo user tidak ditemukan");
        //            return false;
        //        }

        //        System.Diagnostics.Debug.WriteLine($"Saldo sebelum: {userSaldo.Saldo}");

        //        // Validasi saldo cukup
        //        if (userSaldo.Saldo < totalAmount)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Saldo tidak mencukupi");
        //            return false;
        //        }

        //        // Kurangi saldo untuk SEMUA metode pembayaran
        //        userSaldo.Saldo -= totalAmount;
        //        _context.UserSaldos.Update(userSaldo);
        //        System.Diagnostics.Debug.WriteLine($"Saldo setelah: {userSaldo.Saldo}");

        //        // Simpan bukti pembayaran jika diperlukan
        //        string proofImagePath = null;
        //        if (paymentMethod != "cod" && !string.IsNullOrEmpty(proofImage))
        //        {
        //            proofImagePath = await SaveProofImage(proofImage, orderId);
        //            if (proofImagePath == null) return false;
        //        }

        //        // Buat record pembayaran
        //        var payment = new Payment
        //        {
        //            PaymentMethod = paymentMethod,
        //            BankName = paymentMethod == "bank-transfer" ? selectedBank : null,
        //            PaymentDate = DateTime.Now,
        //            PaymentStatus = paymentMethod == "cod"
        //                ? GeneralPaymentStatusData.Pending
        //                : GeneralPaymentStatusData.Completed,
        //            TotalAmount = totalAmount,
        //            ProofImage = proofImagePath
        //        };

        //        _context.Payments.Add(payment);
        //        await _context.SaveChangesAsync();

        //        // set paymentId di order
        //        order.PaymentId = payment.Id;
        //        _context.Orders.Update(order);

        //        // Update status order
        //        order.Status = paymentMethod == "cod"
        //            ? GeneralOrderStatusData.Processing
        //            : GeneralOrderStatusData.Shipped;

        //        // simpan lagi setelah update order
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        System.Diagnostics.Debug.WriteLine("Pembayaran berhasil diproses");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        //        return false;
        //    }
        //}

        //public async Task<string> SaveProofImage(string base64Image, int orderId)
        //{
        //    try
        //    {
        //        var base64Data = base64Image.Substring(base64Image.IndexOf(',') + 1);
        //        var bytes = Convert.FromBase64String(base64Data);

        //        var fileName = $"payment_proof_{orderId}_{Guid.NewGuid()}.jpg";
        //        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "payment-proofs");

        //        // Pastikan folder ada
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        var filePath = Path.Combine(uploadsFolder, fileName);
        //        await File.WriteAllBytesAsync(filePath, bytes);

        //        return $"/uploads/payment-proofs/{fileName}";
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error saving proof: {ex.Message}");
        //        throw new Exception("Terjadi Kesalahan saat Payment", ex);
        //    }
        //}
    }
}
