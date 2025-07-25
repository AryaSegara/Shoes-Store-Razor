using Microsoft.AspNetCore.Mvc;
using Shoes_Store.Interface;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory _category;

        public CategoryController(ICategory category)
        {
            _category = category;
        }
        public IActionResult Index()
        {
            var data = _category.GetListCategory();
            return View(data);
        }

        public IActionResult Update(int id)
        {
            var data = _category.GetCategoryById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(CategoryDTO categoryDTO)
        {
            if (categoryDTO.Id == 0)
            {
                var data = _category.AddCategory(categoryDTO);
                if (data)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                var data = _category.EditCategory(categoryDTO);
                if (data)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View();

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var data = _category.DeleteCategory(id);
            if (data)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Gagal menghapus supplier.");
        }

    }
}
