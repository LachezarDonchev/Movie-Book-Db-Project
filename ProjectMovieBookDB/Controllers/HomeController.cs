using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectMovieBookDB.Models;

namespace ProjectMovieBookDB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Конструктор на контролера, който приема логър за запис на събития.
    /// </summary>
    /// <param name="logger">Логър за запис на събития в приложението.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Действие, което връща началната страница (Index).
    /// </summary>
    /// <returns>Връща изглед за началната страница.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Действие, което връща страница с информация за поверителност.
    /// </summary>
    /// <returns>Връща изглед за страницата с политика за поверителност.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Действие, което показва страницата за грешки.
    /// </summary>
    /// <returns>Връща изглед за грешки с уникален идентификатор на заявката.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}