using iammovies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iammovies.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmesApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFilmes()
        {
            var filmes = await _context.Filmes.ToListAsync();
            return Ok(filmes);
        }

        [HttpPost]
        public async Task<IActionResult> AddFilme([FromBody] Filme filme)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();
            return Ok(filme);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
                return NotFound();

            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditFilme(int id, [FromBody] Filme filme)
        {
            if (id != filme.Id || !ModelState.IsValid)
                return BadRequest();

            _context.Entry(filme).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}