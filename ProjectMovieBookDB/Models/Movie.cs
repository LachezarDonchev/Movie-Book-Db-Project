using System.ComponentModel.DataAnnotations;

namespace ProjectMovieBookDB.Models;

/// <summary>
/// Represents a movie in the system. Each movie has a director and genres.
/// </summary>
public class Movie
{
    /// <summary>
    /// The ID of the movie.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The title of the movie.
    /// </summary>
    [Required(ErrorMessage = "Movie title is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Movie title should be between 3 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the director of the movie.
    /// </summary>
    public int DirectorId { get; set; }

    /// <summary>
    /// The director of the movie.
    /// </summary>
    public Director Director { get; set; } = null!;

    /// <summary>
    /// The list of genres associated with the movie.
    /// </summary>
    [Required(ErrorMessage = "At least one genre must be associated with the movie")]
    public List<Genre> Genres { get; set; } = new();
}