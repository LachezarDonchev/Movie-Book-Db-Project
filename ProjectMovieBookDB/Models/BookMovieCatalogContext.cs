using Microsoft.EntityFrameworkCore;

/// <summary>
/// The database context class that represents all tables and relationships in the system.
/// </summary>
namespace ProjectMovieBookDB.Models;

public class BookMovieCatalogContext : DbContext
{
    /// <summary>
    /// The collection of books in the database.
    /// </summary>
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// The collection of movies in the database.
    /// </summary>
    public DbSet<Movie> Movies { get; set; }

    /// <summary>
    /// The collection of authors in the database.
    /// </summary>
    public DbSet<Author> Authors { get; set; }

    /// <summary>
    /// The collection of directors in the database.
    /// </summary>
    public DbSet<Director> Directors { get; set; }

    /// <summary>
    /// The collection of genres in the database.
    /// </summary>
    public DbSet<Genre> Genres { get; set; }

    /// <summary>
    /// Configures the connection string and options for the database.
    /// </summary>
    /// <param name="options">Options for configuration context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BookCatalogDB;Trusted_Connection=True;");

    /// <summary>
    /// Configures the modeling of tables in the database.
    /// </summary>
    /// <param name="modelBuilder">Модел, който позволява конфигурирането на връзките между обектите и базата данни.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the relationship between books and authors (one-to-many)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId);

        // Configure the relationship between movies and directors (one-to-many)
        modelBuilder.Entity<Movie>()
               .HasOne(m => m.Director)
               .WithMany(d => d.Movies)
               .HasForeignKey(m => m.DirectorId);

        // Configure the many-to-many relationship between books and genres
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Genres)
            .WithMany(g => g.Books);

        // Configure the many-to-many relationship between movies and genres
        modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies);
    }
}