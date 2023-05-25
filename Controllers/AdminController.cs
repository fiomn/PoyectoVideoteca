using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVideoteca.Data;
using System.Data;

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "admin")] //just admin can use this controller
    public class AdminController : Controller
    {
        private TestUCRContext db= new TestUCRContext();
        public ActionResult AdminMain()
        {
            return View();
        }
    }
}
