using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorsController : ControllerBase
{
    private readonly BookMovieCatalogContext _context;

    /// <summary>
    /// Constructor of the controller that accepts the database context.
    /// </summary>
    /// <param name="context">Database context for working with directors.</param>
    public DirectorsController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all directors from the database.
    /// </summary>
    /// <returns>List of all directors and their movies.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
    {
        return await _context.Directors.Include(d => d.Movies).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific director by ID.
    /// </summary>
    /// <param name="id">ID of the director to be retrieved.</param>
    /// <returns>Director with their movies or NotFound if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Director>> GetDirector(int id)
    {
        var director = await _context.Directors.Include(d => d.Movies)
                        .FirstOrDefaultAsync(d => d.Id == id);

        if (director == null)
            return NotFound();

        return director;
    }

    /// <summary>
    /// Creates a new director in the database.
    /// </summary>
    /// <param name="director">Object containing information about the new director.</param>
    /// <returns>Newly created director with status 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Director>> CreateDirector(Director director)
    {
        _context.Directors.Add(director);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director);
    }

    /// <summary>
    /// Updates an existing director by ID.
    /// </summary>
    /// <param name="id">ID of the director to be updated.</param>
    /// <param name="director">Object containing the updated director information.</param>
    /// <returns>NoContent (HTTP 204) after successful update.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDirector(int id, Director director)
    {
        if (id != director.Id)
            return BadRequest();

        _context.Entry(director).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a director from the database by ID.
    /// </summary>
    /// <param name="id">ID of the director to be deleted.</param>
    /// <returns>NoContent (HTTP 204) after successful deletion.</returns>
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

    /// <summary>
    /// Retrieves a list of directors. Optionally filters the list based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term used to filter directors by name.</param>
    /// <returns>A list of directors matching the search criteria, or all directors if no search term is provided.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Director>>> GetDirectors(string? searchTerm)
    {
        var directorsQuery = _context.Directors.Include(d => d.Movies).AsQueryable();

        // If a search term is provided, filter directors by name
        if (!string.IsNullOrEmpty(searchTerm))
        {
            directorsQuery = directorsQuery.Where(d => d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var directors = await directorsQuery.ToListAsync();
        return directors;
    }
}
