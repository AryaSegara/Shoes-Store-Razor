using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IProduct
    {
        public List<ProductDTO> GetListProduct();
        public Product GetProductById(int id);
        public bool EditProduct(ProductDTO productDTO);
        public bool DeleteProduct(int id);
        public bool AddProduct(ProductDTO productDTO);
        public List<SelectListItem> Products();
    }
}
