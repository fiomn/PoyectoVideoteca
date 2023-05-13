using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "admin")] //just admin can use this controller
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
