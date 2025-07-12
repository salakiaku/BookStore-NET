using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.API.Data;
using BookStore.API.Models.Book;
using AutoMapper;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookStoreDbContext context, IMapper mapper, ILogger<BooksController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookGetListDTO>>> GetBooks()
        {
            try
            {
                var books = await _context.Books.Include(a=> a.Author).ToListAsync();
                var mappedBooks = _mapper.Map<IEnumerable<BookGetListDTO>>(books);

               return Ok(mappedBooks);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,$"Falha ao pegar registro no servidor {nameof(GetBooks)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Falha ao pegar os dados no servidor");
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BooksGetDetailsDTO>> GetBook(int id)
        {

            try
            {
                var book = await _context.Books.Include(a=> a.Author).FirstOrDefaultAsync(b=> b.Id == id);
                if (book == null)
                {
                    _logger.LogWarning($"Nenhum registro encontrado {nameof(GetBook)} Id {id}");
                    return NotFound();
                }
                var mapBook = _mapper.Map<BooksGetDetailsDTO>(book);

                return Ok (mapBook);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,$"Falha ao  Pegar registros no servidor {nameof(GetBook)}");
                return BadRequest("Falha ao  Pegar registros no servidor!");
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO book)
        {
            if (id != book.Id)
            {
                _logger.LogWarning($"Falha bao actualizar os dados {nameof(PutBook)} os id são diferentes ");
                return BadRequest();
            }
            var bookModel = _mapper.Map<Book>(book);
            _context.Entry(bookModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BookExists(id))
                {
                    _logger.LogWarning($"Falha ao actualizar o registro {nameof(PutBook)} o Id {id} não existe!");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"Falha ao actualizar o registro {nameof(PutBook)}");
                    return BadRequest("Falha ao actualizar o registro");

                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Falha ao actualizar o registro {nameof(PutBook)}");
                return BadRequest("Falha ao actualizar o registro");
            }


            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookCreateDTO book)
        {
            try
            {
                var bookModel = _mapper.Map<Book>(book);
                _context.Books.Add(bookModel);
                bookModel.Id = await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = bookModel.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Falha ao criar registro no servidor {nameof(PostBook)} !");
                return BadRequest("Falha ao criar registro no servidor");

            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning( $"Registro não enconrad {nameof(DeleteBook)} - Id {id} ");

                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Falha ao eliminar registro  no servidor {nameof(DeleteBook)}");
                return BadRequest("Falha ao eliminar registro  no servidor");
            }
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
