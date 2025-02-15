namespace ProjectMovieBookDB.Models;

public class ErrorViewModel
{
    /// <summary>
    /// The request identifier used for tracking errors.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Determines whether the request identifier should be displayed.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}