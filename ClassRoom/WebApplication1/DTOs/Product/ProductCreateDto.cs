using WebApplication1.DTOs.Image;

namespace WebApplication1.DTOs.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> UploadImages { get; set; }
    }
}
