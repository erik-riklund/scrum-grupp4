using App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  [Guardian]
  public partial class CartController(ISessionManager session) : Controller { }
}
