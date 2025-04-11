using System.Diagnostics.Metrics;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Category;
using WebApplication1.DTOs.Image;
using WebApplication1.DTOs.Product;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public CategoryController(AppDbContext appDbContext ,
                                  IMapper mapper)
        {
            _context = appDbContext;   
            _mapper=mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var countries = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            var countriesDto = countries.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                
               Products= c.Products.Select(b => new ProductDto
                {
                    Id = b.Id,
                    Name = b.Name,
                   Description = b.Description,
                   Price = b.Price
                }).ToList()
            }).ToList();

            return Ok(countriesDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            await _context.Categories.AddAsync(_mapper.Map<Category>(request));
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), "Created success");
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var data = await _context.Categories.FindAsync(id);
            if (data is null) return NotFound();
            _context.Categories.Remove(data);
            await _context.SaveChangesAsync();
            return Ok();

        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] CategoryEditDto category)
        {
            var categories = await _context.Categories
          .Include(c => c.Products).FirstAsync(n => n.Id == id);
            if (categories == null) return NotFound();
            categories.Name = category.Name;
            await _context.SaveChangesAsync();
            return Ok(category);

        }
    }
}
