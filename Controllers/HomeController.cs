using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;
using System.Diagnostics;

namespace App.Controllers
{
  public class HomeController : Controller
  {
    public async Task<IActionResult> Index()
    {
      var movie = new Movie { Title = "The Matrix" };
      var actor = new Actor { Name = "Keanu Reeves" };
      
      await movie.SaveAsync();
      await actor.SaveAsync();

      await movie.Actors.AddAsync(actor);
      await actor.Movies.AddAsync(movie);

      ViewBag.Movies = (await DB.Find<Movie>().ExecuteCursorAsync()).ToEnumerable();

      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
