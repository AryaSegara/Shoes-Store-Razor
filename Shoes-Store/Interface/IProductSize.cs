using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface IProductSize
    {
        public List<ProductSizeDTO> GetlistProductSize();
        public ProductSize GetProductSizeById(int id);
        public bool EditProductSize(ProductSizeDTO productSizeDTO);
        public bool DeleteProductSize(int id);
        public bool AddProductSize(ProductSizeDTO productSize);
        public List<SelectListItem> ProductSizes();

    }
}
