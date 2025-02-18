using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorsController : ControllerBase
{
    private readonly BookMovieCatalogContext _context;

    public DirectorsController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
    {
        return await _context.Directors.Include(d => d.Movies).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Director>> GetDirector(int id)
    {
        var director = await _context.Directors.Include(d => d.Movies)
                        .FirstOrDefaultAsync(d => d.Id == id);
        if (director == null)
            return NotFound();

        return director;
    }

    [HttpPost]
    public async Task<ActionResult<Director>> CreateDirector(Director director)
    {
        _context.Directors.Add(director);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDirector(int id, Director director)
    {
        if (id != director.Id)
            return BadRequest();

        _context.Entry(director).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDirector(int id)
    {
        var director = await _context.Directors.FindAsync(id);
        if (director == null)
            return NotFound();

        _context.Directors.Remove(director);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}