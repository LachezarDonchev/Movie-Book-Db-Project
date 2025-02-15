using Microsoft.AspNetCore.Mvc;

namespace ProjectMovieBookDB.Controllers;

public class BooksController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}