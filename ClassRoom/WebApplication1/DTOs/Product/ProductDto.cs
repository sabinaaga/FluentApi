using WebApplication1.DTOs.Image;

namespace WebApplication1.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public List<ImageDto> Images { get; set; }
    }
}
