using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class ProductSizeController : Controller
    {
        private readonly IProductSize _productSize;
        private readonly IProduct _product;

        public ProductSizeController(IProductSize productSize, IProduct product)
        {
            _productSize = productSize;
            _product = product;
        }

        // GET
        public IActionResult Index()
        {
            var data = _productSize.GetlistProductSize();
            return View(data);
        }

        //UPDATE
        public IActionResult Update(int id)
        {
            ViewBag.Product = _product.Products();
            var data = _productSize.GetProductSizeById(id);
            return View(data);
        }

        // UPDATE
        [HttpPost]
        public IActionResult Update(ProductSizeDTO productSizeDTO)
        {
            if (productSizeDTO.Id == 0)
            {
                var data = _productSize.AddProductSize(productSizeDTO);
                if (data)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                var data = _productSize.EditProductSize(productSizeDTO);
                if (data)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View();

        }

        //DELETE
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _productSize.DeleteProductSize(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus Size.");
        }

    }
}
