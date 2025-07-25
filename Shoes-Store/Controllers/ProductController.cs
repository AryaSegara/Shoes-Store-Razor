using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProduct _product;
        private readonly ICategory _category;

        public ProductController(IProduct product, ICategory category)
        {
            _product = product;
            _category = category;
        }
        public IActionResult Index()
        {
            var data = _product.GetListProduct();
            return View(data);
        }




        [HttpPost]
        public IActionResult Edit(ProductDTO productDto)
        {
            if (productDto.Id == 0)
            {
                _product.AddProduct(productDto);
            }
            else
            {
                _product.EditProduct(productDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Category = _category.Categories();
            var data = _product.GetProductById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _product.DeleteProduct(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus produk.");
        }

    }
}
