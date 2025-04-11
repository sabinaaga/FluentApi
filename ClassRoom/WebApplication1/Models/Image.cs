namespace WebApplication1.Models
{
    public class Image:BaseEntity
    {
        public string ProductImage { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public Product ProductName { get; set; }
    }
}
