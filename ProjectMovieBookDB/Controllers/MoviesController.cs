using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly BookMovieCatalogContext _context;

    /// <summary>
    /// Constructor of the controller that accepts the database context.
    /// </summary>
    /// <param name="context">Database context for working with movies.</param>
    public MoviesController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all movies from the database.
    /// </summary>
    /// <returns>A list of all movies, including the director and genres.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        return await _context.Movies.Include(m => m.Director).Include(m => m.Genres).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific movie by ID.
    /// </summary>
    /// <param name="id">The ID of the movie to be retrieved.</param>
    /// <returns>A movie with its director and genres, or NotFound if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies.Include(m => m.Director).Include(m => m.Genres)
                        .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
            return NotFound();

        return movie;
    }

    /// <summary>
    /// Creates a new movie in the database.
    /// </summary>
    /// <param name="movie">An object with information about the new movie.</param>
    /// <returns>The newly created movie with status 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    /// <summary>
    /// Updates an existing movie by ID.
    /// </summary>
    /// <param name="id">The ID of the movie to be updated.</param>
    /// <param name="movie">An object with the new information for the movie.</param>
    /// <returns>NoContent (HTTP 204) after a successful update.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie movie)
    {
        if (id != movie.Id)
            return BadRequest();

        _context.Entry(movie).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a movie from the database by ID.
    /// </summary>
    /// <param name="id">The ID of the movie to be deleted.</param>
    /// <returns>NoContent (HTTP 204) after successful deletion.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
            return NotFound();

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of movies. Optionally filters the list based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term used to filter movies by title or director's name.</param>
    /// <returns>A list of movies matching the search criteria, or all movies if no search term is provided.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies(string? searchTerm)
    {
        var moviesQuery = _context.Movies.Include(m => m.Director).Include(m => m.Genres).AsQueryable();

        // If a search term is provided, filter movies by title or director's name
        if (!string.IsNullOrEmpty(searchTerm))
        {
            moviesQuery = moviesQuery.Where(m => m.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                  m.Director.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var movies = await moviesQuery.ToListAsync();
        return movies;
    }
}
