using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly BookMovieCatalogContext _context;

    /// <summary>
    /// Конструктор на контролера, който приема контекста на базата данни.
    /// </summary>
    /// <param name="context">Контекст на базата данни за работа с автори.</param>
    public AuthorsController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Извлича всички автори от базата данни.
    /// </summary>
    /// <returns>Списък с всички автори и техните книги.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        return await _context.Authors.Include(a => a.Books).ToListAsync();
    }

    /// <summary>
    /// Извлича конкретен автор по ID.
    /// </summary>
    /// <param name="id">ID на автора, който ще се извлече.</param>
    /// <returns>Автор с неговите книги или NotFound, ако не е намерен.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var author = await _context.Authors.Include(a => a.Books)
                        .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
            return NotFound();

        return author;
    }

    /// <summary>
    /// Създава нов автор в базата данни.
    /// </summary>
    /// <param name="author">Обект с информация за новия автор.</param>
    /// <returns>Новосъздаден автор със статус 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    /// <summary>
    /// Актуализира съществуващ автор по ID.
    /// </summary>
    /// <param name="id">ID на автора, който ще се актуализира.</param>
    /// <param name="author">Обект с новата информация за автора.</param>
    /// <returns>NoContent (HTTP 204) след успешна актуализация.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, Author author)
    {
        if (id != author.Id)
            return BadRequest();

        _context.Entry(author).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Изтрива автор от базата данни по ID.
    /// </summary>
    /// <param name="id">ID на автора, който ще бъде изтрит.</param>
    /// <returns>NoContent (HTTP 204) след успешно изтриване.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
            return NotFound();

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of authors. Optionally filters the list based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term used to filter authors by name.</param>
    /// <returns>A list of authors matching the search criteria, or all authors if no search term is provided.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors(string? searchTerm)
    {
        var authorsQuery = _context.Authors.Include(a => a.Books).AsQueryable();

        // If a search term is provided, filter authors by name
        if (!string.IsNullOrEmpty(searchTerm))
        {
            authorsQuery = authorsQuery.Where(a => a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var authors = await authorsQuery.ToListAsync();
        return authors;
    }
}