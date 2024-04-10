using App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public partial class AccountController(IAuthenticator authenticator) : Controller { }
}
