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
    /// Конструктор на контролера, който приема контекста на базата данни.
    /// </summary>
    /// <param name="context">Контекст на базата данни за работа с режисьори.</param>
    public DirectorsController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Извлича всички режисьори от базата данни.
    /// </summary>
    /// <returns>Списък с всички режисьори и техните филми.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
    {
        return await _context.Directors.Include(d => d.Movies).ToListAsync();
    }

    /// <summary>
    /// Извлича конкретен режисьор по ID.
    /// </summary>
    /// <param name="id">ID на режисьора, който ще се извлече.</param>
    /// <returns>Режисьор с неговите филми или NotFound, ако не е намерен.</returns>
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
    /// Създава нов режисьор в базата данни.
    /// </summary>
    /// <param name="director">Обект с информация за новия режисьор.</param>
    /// <returns>Новосъздаден режисьор със статус 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Director>> CreateDirector(Director director)
    {
        _context.Directors.Add(director);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director);
    }

    /// <summary>
    /// Актуализира съществуващ режисьор по ID.
    /// </summary>
    /// <param name="id">ID на режисьора, който ще се актуализира.</param>
    /// <param name="director">Обект с новата информация за режисьора.</param>
    /// <returns>NoContent (HTTP 204) след успешна актуализация.</returns>
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
    /// Изтрива режисьор от базата данни по ID.
    /// </summary>
    /// <param name="id">ID на режисьора, който ще бъде изтрит.</param>
    /// <returns>NoContent (HTTP 204) след успешно изтриване.</returns>
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