using System.IO;

namespace ProjectMovieBookDB.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DirectorId { get; set; }
    public Director Director { get; set; } = null!;
    public List<Genre> Genres { get; set; } = new();
}