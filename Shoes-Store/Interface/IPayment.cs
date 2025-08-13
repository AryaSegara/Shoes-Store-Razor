using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IPayment
    {
        public List<PaymentDTO> GetListPayment();
        public Payment GetPaymentlById(int id);
        public bool DeletePayment(int id);
        //Task<bool> ConfirmPayment(int orderId, string paymentMethod, string selectedBank, string proofImage);
        //Task<string> SaveProofImage(string base64Image, int orderId);
    }
}
