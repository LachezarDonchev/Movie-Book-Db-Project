namespace ProjectMovieBookDB.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public List<Genre> Genres { get; set; } = new();
}