using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBookDB.Models;

/// <summary>
/// Represents a book in the system. Each book has an author and genres.
/// </summary>
public class Book
{
    /// <summary>
    /// The ID of the book.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The title of the book.
    /// </summary>
    [Required(ErrorMessage = "Book title is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Book title should be between 3 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the author of the book.
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// The author of the book.
    /// </summary>
    public Author Author { get; set; } = null!;

    /// <summary>
    /// The list of genres associated with the book.
    /// </summary>
    [Required(ErrorMessage = "At least one genre must be associated with the book")]
    public List<Genre> Genres { get; set; } = new();
}