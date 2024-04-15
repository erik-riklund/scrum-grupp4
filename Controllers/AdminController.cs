using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  [Guardian(Roles = "Admin")]
  public partial class AdminController : Controller {}
}