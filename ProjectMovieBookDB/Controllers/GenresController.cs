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
    /// Конструктор на контролера, който приема контекста на базата данни.
    /// </summary>
    /// <param name="context">Контекст на базата данни за работа с жанрове.</param>
    public GenresController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Извлича всички жанрове от базата данни.
    /// </summary>
    /// <returns>Списък с всички жанрове, включително свързаните с тях книги и филми.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
    {
        return await _context.Genres.Include(g => g.Books).Include(g => g.Movies).ToListAsync();
    }

    /// <summary>
    /// Извлича конкретен жанр по ID.
    /// </summary>
    /// <param name="id">ID на жанра, който ще се извлече.</param>
    /// <returns>Жанр с неговите свързани книги и филми или NotFound, ако не е намерен.</returns>
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
    /// Създава нов жанр в базата данни.
    /// </summary>
    /// <param name="genre">Обект с информация за новия жанр.</param>
    /// <returns>Новосъздаден жанр със статус 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Genre>> CreateGenre(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }

    /// <summary>
    /// Актуализира съществуващ жанр по ID.
    /// </summary>
    /// <param name="id">ID на жанра, който ще се актуализира.</param>
    /// <param name="genre">Обект с новата информация за жанра.</param>
    /// <returns>NoContent (HTTP 204) след успешна актуализация.</returns>
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
    /// Изтрива жанр от базата данни по ID.
    /// </summary>
    /// <param name="id">ID на жанра, който ще бъде изтрит.</param>
    /// <returns>NoContent (HTTP 204) след успешно изтриване.</returns>
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