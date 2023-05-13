using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoVideoteca.Controllers
{
    [Authorize] //everyone can use this controller
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
