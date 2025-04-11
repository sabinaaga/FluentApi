namespace WebApplication1.DTOs.Image
{
    public class ImageDto
    {
        public string Id { get; set; }
        public IFormFile ProductImage { get; set; }
        public IFormFile IsMain { get; set; }
    }
}
