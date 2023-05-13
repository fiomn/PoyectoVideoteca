using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "superAdmin")] //just superAdmin can use this controller
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
