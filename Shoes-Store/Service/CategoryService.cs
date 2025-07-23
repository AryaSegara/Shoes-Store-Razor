using Microsoft.AspNetCore.Mvc.Rendering;
using Shoes_Store.Interface;
using Shoes_Store.Models;
using Shoes_Store.Models.DB;
using Shoes_Store.Models.DTO;

namespace Shoes_Store.Service
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationContext _context;

        public CategoryService(ApplicationContext context)
        {
            _context = context;
        }

        public List<CategoryDTO> GetListCategory()
        {
            var data = _context.Categories.Select(x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name,

            }).ToList();
            return data;

        }

        public Category GetCategoryById(int id)
        {
            var data = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return new Category();
            }

            return data;
        }

        public bool EditCategory(CategoryDTO categoryDTO)
        {
            var data = _context.Categories.FirstOrDefault(x => x.Id == categoryDTO.Id);
            if (data == null)
            {
                return false;
            }
            data.Id = categoryDTO.Id;
            data.Name = categoryDTO.Name;

            _context.Categories.Update(data);
            _context.SaveChanges();
            return true;
        }

        public bool AddCategory(CategoryDTO categoryDTO)
        {
            var data = new Category();
            data.Name = categoryDTO.Name;

            _context.Categories.Add(data);
            _context.SaveChanges();
            return true;

        }

        public bool DeleteCategory(int id)
        {
            var data = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return false;
            }

            _context.Categories.Remove(data);
            _context.SaveChanges();
            return true;
        }

        public List<SelectListItem> Categories()
        {
            var datas = _context.Categories
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            return datas;
        }
    }
}
