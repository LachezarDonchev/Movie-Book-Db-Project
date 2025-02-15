using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Constructor of the controller that accepts a logger for event logging.
    /// </summary>
    /// <param name="logger">A logger for logging events in the application.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Action that returns the home page (Index).
    /// </summary>
    /// <returns>Returns the view for the home page.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Action that returns the privacy policy page.
    /// </summary>
    /// <returns>Returns the view for the privacy policy page.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Action that shows the error page.
    /// </summary>
    /// <returns>Returns the view for the error page with a unique request identifier.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
