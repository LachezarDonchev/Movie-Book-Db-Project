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
    /// Конструктор на контролера, който приема контекста на базата данни.
    /// </summary>
    /// <param name="context">Контекст на базата данни за работа с книги.</param>
    public BooksController(BookMovieCatalogContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Извлича всички книги от базата данни.
    /// </summary>
    /// <returns>Списък с всички книги и техните автори и жанрове.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await _context.Books.Include(b => b.Author).Include(b => b.Genres).ToListAsync();
    }

    /// <summary>
    /// Извлича конкретна книга по ID.
    /// </summary>
    /// <param name="id">ID на книгата, която ще се извлече.</param>
    /// <returns>Книга с нейния автор и жанрове или NotFound, ако не е намерена.</returns>
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
    /// Създава нова книга в базата данни.
    /// </summary>
    /// <param name="book">Обект с информация за новата книга.</param>
    /// <returns>Новосъздадена книга със статус 201 Created.</returns>
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    /// <summary>
    /// Актуализира съществуваща книга по ID.
    /// </summary>
    /// <param name="id">ID на книгата, която ще се актуализира.</param>
    /// <param name="book">Обект с новата информация за книгата.</param>
    /// <returns>NoContent (HTTP 204) след успешна актуализация.</returns>
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
    /// Изтрива книга от базата данни по ID.
    /// </summary>
    /// <param name="id">ID на книгата, която ще бъде изтрита.</param>
    /// <returns>NoContent (HTTP 204) след успешно изтриване.</returns>
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