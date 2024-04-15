using App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Reflection;

namespace App.Controllers
{
    public partial class OrderController(ISessionManager session) : Controller { }
}
