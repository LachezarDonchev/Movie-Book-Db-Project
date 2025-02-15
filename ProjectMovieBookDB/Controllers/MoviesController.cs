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
    /// Конструктор на контролера, който приема контекста на базата данни.
    /// </summary>
    /// <param name="context">Контекст на базата данни за работа с филми.</param>
    public MoviesController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Извлича всички филми от базата данни.
    /// </summary>
    /// <returns>Списък с всички филми, включително режисьора и жанровете.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        return await _context.Movies.Include(m => m.Director).Include(m => m.Genres).ToListAsync();
    }

    /// <summary>
    /// Извлича конкретен филм по ID.
    /// </summary>
    /// <param name="id">ID на филма, който ще се извлече.</param>
    /// <returns>Филм с неговите режисьор и жанрове или NotFound, ако не е намерен.</returns>
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
    /// Създава нов филм в базата данни.
    /// </summary>
    /// <param name="movie">Обект с информация за новия филм.</param>
    /// <returns>Новосъздаден филм със статус 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    /// <summary>
    /// Актуализира съществуващ филм по ID.
    /// </summary>
    /// <param name="id">ID на филма, който ще се актуализира.</param>
    /// <param name="movie">Обект с новата информация за филма.</param>
    /// <returns>NoContent (HTTP 204) след успешна актуализация.</returns>
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
    /// Изтрива филм от базата данни по ID.
    /// </summary>
    /// <param name="id">ID на филма, който ще бъде изтрит.</param>
    /// <returns>NoContent (HTTP 204) след успешно изтриване.</returns>
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