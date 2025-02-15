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
    /// Constructor of the controller that accepts the database context.
    /// </summary>
    /// <param name="context">Database context for working with authors.</param>
    public AuthorsController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all authors from the database.
    /// </summary>
    /// <returns>List of all authors and their books.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        return await _context.Authors.Include(a => a.Books).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific author by ID.
    /// </summary>
    /// <param name="id">ID of the author to be retrieved.</param>
    /// <returns>Author with their books or NotFound if not found.</returns>
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
    /// Creates a new author in the database.
    /// </summary>
    /// <param name="author">Object containing information about the new author.</param>
    /// <returns>Newly created author with status 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    /// <summary>
    /// Updates an existing author by ID.
    /// </summary>
    /// <param name="id">ID of the author to be updated.</param>
    /// <param name="author">Object containing the updated author information.</param>
    /// <returns>NoContent (HTTP 204) after successful update.</returns>
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
    /// Deletes an author from the database by ID.
    /// </summary>
    /// <param name="id">ID of the author to be deleted.</param>
    /// <returns>NoContent (HTTP 204) after successful deletion.</returns>
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
