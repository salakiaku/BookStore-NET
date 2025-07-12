using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Author;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorGetDTO>>> GetAuthors()
        {
            try
            {

                var mappedmodel = _mapper.Map<IEnumerable<AuthorGetDTO>>(await _context.Authors.ToListAsync());

                if (!mappedmodel.Any())
                    _logger.LogWarning($"Nenhum registro encontrado {nameof(GetAuthors)}");
                return Ok(mappedmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro ao tentar pegar os dados no servidor {nameof(GetAuthors)}!");
                return StatusCode(500, "Ocorreu um erro ao tentar pegar os dados no servidor");
            }
            
             
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"Nenhum registro encontrado {nameof(GetAuthor)}");
                    return NotFound();
                }
                var mappedmodel = _mapper.Map<AuthorGetDTO>(author);
                return Ok(mappedmodel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falha ao pegar os dados no servidor {nameof(GetAuthor)}");
                throw;
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDTO author)
        {
           

            try
            {
                if (id != author.Id)
                {
                    return BadRequest();
                }

                var mappedModel = _mapper.Map<Author>(author);
                _context.Entry(mappedModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                return NoContent();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AuthorExists(id))
                {
                    _logger.LogWarning($"Nenhum registro encontrado  {nameof(PutAuthor)}");
                    return NotFound();
                }
                else
                {
                   _logger.LogError(ex, $"Ocorreu um erro Actualizar os dados no servidor {nameof(PutAuthor)}");
                    return BadRequest();
                }

            }
         

        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDTO>> PostAuthor(AuthorCreateDTO author)
        {
            try
            {
                var mappModel = _mapper.Map<Author>(author);
                _context.Authors.Add(mappModel);

                mappModel.Id = await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = mappModel.Id }, author);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Ocorreu um erro ao tentar salvar dados no servidor {nameof(PostAuthor)}");
                return BadRequest("Ocorreu um erro ao tentar salvar dados no servidor");
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"Nenhum registro encontrado {nameof(DeleteAuthor)}");
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao eliminar o registro no servidor {nameof(DeleteAuthor)}");
                return BadRequest("Ocorreu um erro ao eliminar o registro no servidor");
                
            }
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
