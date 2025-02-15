namespace ProjectMovieBookDB.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Book> Books { get; set; } = new();
    public List<Movie> Movies { get; set; } = new();
}