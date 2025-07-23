using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Service
{
    public class ProductSizeService : IProductSize
    {
        private readonly ApplicationContext _context;

        public ProductSizeService(ApplicationContext context)
        {
            _context = context;
        }

        public List<ProductSizeDTO> GetlistProductSize()
        {
            var data = _context.ProductSizes.Include(y => y.Product).Select(x => new ProductSizeDTO
            {
                Id = x.Id,
                Size = x.Size,
                Stock = x.Stock,
                ProductName = x.Product.Name,

            }).ToList();
            return data;

        }

        public ProductSize GetProductSizeById(int id)
        {
            var data = _context.ProductSizes.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return new ProductSize();
            }
            return data;
        }


        public bool EditProductSize(ProductSizeDTO productSizeDTO)
        {
            var data = _context.ProductSizes.FirstOrDefault(x => x.Id == productSizeDTO.Id);
            if (data == null)
            {
                return false;
            }
            data.ProductId = productSizeDTO.ProductId;
            data.Size = productSizeDTO.Size;
            data.Stock = productSizeDTO.Stock;

            _context.ProductSizes.Update(data);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteProductSize(int id)
        {
            var data = _context.ProductSizes.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return false;
            }

            _context.ProductSizes.Remove(data);
            _context.SaveChanges();
            return true;
        }

        public bool AddProductSize(ProductSizeDTO productSize)
        {

            var data = new ProductSize();

            data.Size = productSize.Size;
            data.Stock = productSize.Stock;
            data.ProductId = productSize.ProductId;

            _context.ProductSizes.Add(data);
            _context.SaveChanges();
            return true;
        }

        public List<SelectListItem> ProductSizes()
        {
            var datas = _context.ProductSizes
                .Select(x => new SelectListItem
                {
                    Text = x.Size,
                    Value = x.Id.ToString()
                }).ToList();

            return datas;
        }
    }
}
