using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly BookMovieCatalogContext _context;

    public GenresController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
    {
        return await _context.Genres.Include(g => g.Books).Include(g => g.Movies).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(int id)
    {
        var genre = await _context.Genres.Include(g => g.Books).Include(g => g.Movies)
                        .FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null)
            return NotFound();

        return genre;
    }

    [HttpPost]
    public async Task<ActionResult<Genre>> CreateGenre(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGenre(int id, Genre genre)
    {
        if (id != genre.Id)
            return BadRequest();

        _context.Entry(genre).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
            return NotFound();

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}