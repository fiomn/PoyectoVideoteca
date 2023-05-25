using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace ProyectoVideoteca.Controllers
{
    
    public class ClientController : Controller
    {
        [Authorize] //everyone can use this controller
        public ActionResult ClientMain()
        {
            return View();
        }

        
    }
}
