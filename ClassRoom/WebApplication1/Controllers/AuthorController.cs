using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Author;
using WebApplication1.DTOs.Category;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public AuthorController(AppDbContext appDbContext,
                                  IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _context.Authors.ToListAsync();

            var result = authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Address= a.Address,
                Email = a.Email,
                Age = a.Age
            });

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            await _context.Authors.AddAsync(_mapper.Map<Author>(request));
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), "Created success");
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var data = await _context.Authors.FindAsync(id);
            if (data is null) return NotFound();
            _context.Authors.Remove(data);
            await _context.SaveChangesAsync();
            return Ok();

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var author = await _context.Authors.FirstAsync(n => n.Id == id);
            if (author == null) return NotFound();



            return Ok(new AuthorDto
            {
                Id = id,
                FullName = author.FullName,
                Address = author.Address,
                Age = author.Age,
                Email = author.Email,
            });
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditAuthorDto request)
        {
            var authors = await _context.Authors.FirstAsync(n => n.Id == id);
            if (authors == null) return NotFound();
            authors.FullName = request.FullName;
            authors.Address = request.Address;
            authors.Email = request.Email;
            authors.Age = request.Age;
            await _context.SaveChangesAsync();
            return Ok(authors);

        }


        [HttpGet]
        public async Task<IActionResult> Searsh([FromQuery] string srcText)
        {
            var datas = await _context.Authors.Where(n => n.FullName.Trim().ToLower().Contains(srcText.Trim()))
                .ToListAsync();
            return Ok(datas);
        }



    }
}
