using WebApplication1.DTOs.Product;

namespace WebApplication1.DTOs.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
