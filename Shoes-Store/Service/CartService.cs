//using Shoes_Store.Models;
//using Shoes_Store.Models.DTO;

//namespace Shoes_Store.Service
//{
//    public class CartService
//    {
//        private readonly ApplicationContext _context;

//        public CartService(ApplicationContext context)
//        {
//            _context = context;
//        }

//        public List<CartDetailDTO> GetListCartDetail()
//        {
//            var data = _context.CartDetails.Select(x => new CartDetailDTO
//            {
//                Id = x.Id,
//                CartId = x.CartId,
//            })
//        }
//    }
//}
