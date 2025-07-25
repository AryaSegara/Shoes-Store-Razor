using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;
using static Shoes_Store.Models.GeneralStatus;

namespace Shoes_Store.Service
{
    public class ProductService : IProduct
    {
        private readonly ApplicationContext _context;

        public ProductService(ApplicationContext context)
        {
            _context = context;
        }

        public List<ProductDTO> GetListProduct()
        {
            var baseUrl = "/images/";

            var data = _context.Products
                .Include(p => p.Category)
                .Where(x => x.ProductStatus != GeneralStatus.GeneralStatusData.delete)
                .Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    ImageUrl = baseUrl + x.Image,
                    CategoryName = x.Category.Name,
                    ProductStatus = x.ProductStatus
                }).ToList();

            return data;
        }


        //public List<ProductDTO> GetListProduct()
        //{
        //    var baseUrl = "/images/";

        //    var data = _context.Products
        //        .Include(p => p.Category)
        //        .Where(x => x.ProductStatus != GeneralStatus.GeneralStatusData.delete)
        //        .Select(x => new ProductDTO
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            Description = x.Description,
        //            Price = x.Price,
        //            ImageUrl = !string.IsNullOrEmpty(x.Image) ? baseUrl + x.Image 
        //            CategoryName = x.Category != null ? x.Category.Name : "-",
        //            ProductStatus = x.ProductStatus
        //        }).ToList();

        //    return data;
        //}


        public Product GetProductById(int id)
        {
            var data = _context.Products.Where(x => x.Id == id && x.ProductStatus != GeneralStatusData.delete).FirstOrDefault();
            if (data == null)
            {
                return new Product();
            }

            return data;
        }

        public bool EditProduct(ProductDTO productDTO)
        {
            var data = _context.Products.FirstOrDefault(x => x.Id == productDTO.Id);
            if (data == null)
            {
                return false;
            }
            data.CategoryId = productDTO.CategoryId;
            data.Name = productDTO.Name;
            data.Description = productDTO.Description;
            data.Price = productDTO.Price;
            data.ProductStatus = productDTO.ProductStatus;
            data.UpdatedAt = DateTime.Now;

            //proses upload image baru jika ada
            if (productDTO.ImageFile != null && productDTO.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(productDTO.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    throw new InvalidOperationException("File format not supported. Only .jpg, .jpeg, .png, .gif allowed.");

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(folderPath); // aman jika folder sudah ada

                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productDTO.ImageFile.CopyTo(stream);
                }

                data.Image = fileName;
            }

            _context.Products.Update(data);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteProduct(int id)
        {
            var data = _context.Products.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return false;
            }

            data.ProductStatus = GeneralStatusData.delete;
            _context.SaveChanges();
            return true;
        }

        public bool AddProduct(ProductDTO productDTO)
        {
            var data = new Product();

            data.Name = productDTO.Name;
            data.Description = productDTO.Description;
            data.Price = productDTO.Price;
            data.ProductStatus = productDTO.ProductStatus;
            data.CategoryId = productDTO.CategoryId;
            data.CreatedAt = DateTime.Now;

            //proses upload image baru jika ada
            if (productDTO.ImageFile != null && productDTO.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(productDTO.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    throw new InvalidOperationException("File format not supported. Only .jpg, .jpeg, .png, .gif allowed.");

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(folderPath); // aman jika folder sudah ada

                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productDTO.ImageFile.CopyTo(stream);
                }

                data.Image = fileName;
            }


            _context.Products.Add(data);
            _context.SaveChanges();
            return true;

        }

        public List<SelectListItem> Products()
        {
            var datas = _context.Products
                .Where(x => x.ProductStatus == GeneralStatusData.Published)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            return datas;
        }
    }
}
