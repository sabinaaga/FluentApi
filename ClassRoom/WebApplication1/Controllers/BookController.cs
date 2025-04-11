using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Author;
using WebApplication1.DTOs.Book;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public BookController(AppDbContext appDbContext,
                                  IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .ToListAsync();

            var result = books.Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Authors = book.BookAuthors.Select(ba => ba.Author.FullName).ToList()
            });

            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author).FirstAsync(n => n.Id == id);
            if (book == null) return NotFound();



            return Ok(new BookDto
            {
                Id = id,
                Title = book.Title,
                Authors = book.BookAuthors.Select(ba => ba.Author.FullName).ToList()

            });
        }


        [HttpGet("by-author")]
        public async Task<IActionResult> GetBooksByAuthorName([FromQuery] string authorName)
        {
            var books = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Where(b => b.BookAuthors.Any(ba => ba.Author.FullName.Contains(authorName)))
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Authors = b.BookAuthors.Select(ba => ba.Author.FullName).ToList()
                })
                .ToListAsync();

            return Ok(books);
        }


        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(CreateBookDto dto)
        {
            var authorsExist = await _context.Authors
                .Where(a => dto.AuthorIds.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync();

            if (authorsExist.Count != dto.AuthorIds.Count)
                return BadRequest();

            var book = new Book
            {
                Title = dto.Title,
                BookAuthors = dto.AuthorIds.Select(id => new BookAuthor
                {
                    AuthorId = id
                }).ToList()
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var createdBook = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.Id == book.Id);

            var result = new BookDto
            {
                Id = createdBook.Id,
                Title = createdBook.Title,
                Authors = createdBook.BookAuthors.Select(ba => ba.Author.FullName).ToList()
            };

            return CreatedAtAction(nameof(Create), "Created success");
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var data = await _context.Books.FindAsync(id);
            if (data is null) return NotFound();
            _context.Books.Remove(data);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] BookEditDto request)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author).FirstAsync(n => n.Id == id);
            if (book == null) return NotFound();
            book.Title = request.Title;
            _context.BookAuthors.RemoveRange(book.BookAuthors);

            book.BookAuthors = request.AuthorId.Select(authorId => new BookAuthor
            {
                AuthorId = authorId,
                BookId = book.Id
            }).ToList();
            await _context.SaveChangesAsync();
            return Ok(new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Authors = book.BookAuthors.Select(ba => ba.Author.FullName).ToList()
            });

        }



        [HttpGet]
        public async Task<IActionResult> Searsh([FromQuery] string srcText)
        {
            var datas = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author).Where(n => n.Title.Trim().ToLower().Contains(srcText.Trim()))
                .ToListAsync();
            return Ok(datas);
        }


    }
}
