using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Product;
using WebApplication1.Helpers.Extensions;
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;



        public ProductController(AppDbContext appDbContext,
                                  IMapper mapper,
                                  IWebHostEnvironment env)
        {
            _context = appDbContext;
            _mapper = mapper;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.Include(v=>v.Category).ToListAsync();

            var productsDto = products.Select(c => new ProductDto
            {
                Id = c.Id,
                Name = c.Name,
               Description = c.Description,
               Price = c.Price,
               CategoryName=c.Category.Name
               
            }).ToList();

            return Ok(productsDto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto request)
        {

            List<WebApplication1.Models.Image> productImages = new();
            foreach (var item in request.UploadImages)
            {
                string fileName = Guid.NewGuid() + "-" + item.FileName;

                string path = Path.Combine(_env.WebRootPath, "images", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
                   productImages.Add(new WebApplication1.Models.Image { ProductImage = fileName, IsMain = false });
            }
            productImages.FirstOrDefault().IsMain = true;

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
               Images = productImages,
               CategoryId=request.CategoryId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), "Created success");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (id == null) return BadRequest();
            var datas = await _context.Products
            .Include(p => p.Images)
            .ToListAsync();


            var data = datas.FirstOrDefault(m => m.Id == id);
            if (data == null) return NotFound();


            IEnumerable<WebApplication1.Models.Image> images = data.Images.ToList();
            foreach (var image in images)
            {
                string filePath = _env.GenerateFilePath("assets/images", image.ProductImage);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

            }
            _context.Products.Remove(data);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
