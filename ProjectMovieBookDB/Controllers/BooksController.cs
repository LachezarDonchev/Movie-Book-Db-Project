using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookMovieCatalogContext _context;

    /// <summary>
    /// Constructor of the controller that accepts the database context.
    /// </summary>
    /// <param name="context">Database context for working with books.</param>
    public BooksController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all books from the database.
    /// </summary>
    /// <returns>List of all books and their authors and genres.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await _context.Books.Include(b => b.Author).Include(b => b.Genres).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific book by ID.
    /// </summary>
    /// <param name="id">ID of the book to be retrieved.</param>
    /// <returns>Book with its author and genres or NotFound if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _context.Books.Include(b => b.Author).Include(b => b.Genres)
                        .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return NotFound();

        return book;
    }

    /// <summary>
    /// Creates a new book in the database.
    /// </summary>
    /// <param name="book">Object containing information about the new book.</param>
    /// <returns>Newly created book with status 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    /// <summary>
    /// Updates an existing book by ID.
    /// </summary>
    /// <param name="id">ID of the book to be updated.</param>
    /// <param name="book">Object containing the updated book information.</param>
    /// <returns>NoContent (HTTP 204) after successful update.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, Book book)
    {
        if (id != book.Id)
            return BadRequest();

        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a book from the database by ID.
    /// </summary>
    /// <param name="id">ID of the book to be deleted.</param>
    /// <returns>NoContent (HTTP 204) after successful deletion.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of books. Optionally filters the list based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term used to filter books by title or author's name.</param>
    /// <returns>A list of books matching the search criteria, or all books if no search term is provided.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string? searchTerm)
    {
        var booksQuery = _context.Books.Include(b => b.Author).Include(b => b.Genres).AsQueryable();

        // If a search term is provided, filter books by title or author's name
        if (!string.IsNullOrEmpty(searchTerm))
        {
            booksQuery = booksQuery.Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                               b.Author.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var books = await booksQuery.ToListAsync();
        return books;
    }
}
