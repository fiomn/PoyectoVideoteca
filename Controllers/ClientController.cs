using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProyectoVideoteca.Data;
using ProyectoVideoteca.Models;
using ProyectoVideoteca.Models.DTO;
using System.Net.Mail;

namespace ProyectoVideoteca.Controllers
{
    [Authorize] //everyone can use this controller
    public class ClientController : Controller
    {
        private TestUCRContext db = new TestUCRContext(); //database context
        public ActionResult ClientMain()
        {
            return View();
        }

        public ActionResult editProfile()
        {
            var username = new LoginModel().UserName;
            var user = db.tb_USER.FromSqlRaw(@"exec getUserP @USERNAME", new SqlParameter("@USERNAME", username)).ToList().FirstOrDefault();
            return View(user);
        }

        [HttpGet]
        public ActionResult recoveryPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult recoveryPassword(string email)
        {

            return View();
        }
    }
}
