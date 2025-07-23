using System.ComponentModel.DataAnnotations.Schema;
using static Shoes_Store.Models.GeneralStatus;

namespace Shoes_Store.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public int Price { get; set; }
        public IFormFile ImageFile { get; set; } // untuk POST/PUT

        public string ImageUrl { get; set; } // untuk GET
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public GeneralStatusData ProductStatus { get; set; }
    }
}
