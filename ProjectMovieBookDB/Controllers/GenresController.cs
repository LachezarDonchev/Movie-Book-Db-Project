using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly BookMovieCatalogContext _context;

    /// <summary>
    /// Constructor of the controller that accepts the database context.
    /// </summary>
    /// <param name="context">Database context for working with genres.</param>
    public GenresController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all genres from the database.
    /// </summary>
    /// <returns>A list of all genres, including the related books and movies.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
    {
        return await _context.Genres.Include(g => g.Books).Include(g => g.Movies).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific genre by ID.
    /// </summary>
    /// <param name="id">The ID of the genre to be retrieved.</param>
    /// <returns>The genre with its related books and movies, or NotFound if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(int id)
    {
        var genre = await _context.Genres.Include(g => g.Books).Include(g => g.Movies)
                        .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null)
            return NotFound();

        return genre;
    }

    /// <summary>
    /// Creates a new genre in the database.
    /// </summary>
    /// <param name="genre">An object with information about the new genre.</param>
    /// <returns>The newly created genre with status 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Genre>> CreateGenre(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }

    /// <summary>
    /// Updates an existing genre by ID.
    /// </summary>
    /// <param name="id">The ID of the genre to be updated.</param>
    /// <param name="genre">An object with the new information for the genre.</param>
    /// <returns>NoContent (HTTP 204) after a successful update.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGenre(int id, Genre genre)
    {
        if (id != genre.Id)
            return BadRequest();

        _context.Entry(genre).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a genre from the database by ID.
    /// </summary>
    /// <param name="id">The ID of the genre to be deleted.</param>
    /// <returns>NoContent (HTTP 204) after successful deletion.</returns>
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

    /// <summary>
    /// Retrieves a list of genres. Optionally filters the list based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term used to filter genres by name.</param>
    /// <returns>A list of genres matching the search criteria, or all genres if no search term is provided.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres(string? searchTerm)
    {
        var genresQuery = _context.Genres.Include(g => g.Books).Include(g => g.Movies).AsQueryable();

        // If a search term is provided, filter genres by name
        if (!string.IsNullOrEmpty(searchTerm))
        {
            genresQuery = genresQuery.Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var genres = await genresQuery.ToListAsync();
        return genres;
    }
}
