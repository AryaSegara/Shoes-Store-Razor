using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Interface
{
    public interface ICategory
    {
        public List<CategoryDTO> GetListCategory();
        public Category GetCategoryById(int id);
        public bool EditCategory(CategoryDTO categoryDTO);
        public bool AddCategory(CategoryDTO categoryDTO);
        public bool DeleteCategory(int id);
        public List<SelectListItem> Categories();
    }
}
