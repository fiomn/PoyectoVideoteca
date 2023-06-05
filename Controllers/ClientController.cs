using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProyectoVideoteca.Data;
using ProyectoVideoteca.Models;
using ProyectoVideoteca.Models.DTO;
using System.Net;
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

        //recovery password by email
        public static void sendEmail(string email)
        {
            string emailSend = "fio.mn1911@gmail.com";
            string pwdSend = "Navarro19!!!";

            var fromAddress = new MailAddress(emailSend);
            var toAddress = new MailAddress(email);
            string subject = "Password recovery";
            string body = "This is your new password for QStream";

            var smtp = new SmtpClient
            {
                Host = "smt.gamil.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(emailSend, pwdSend)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
            })
            {
                message.IsBodyHtml = false;
                smtp.Send(message);
            }
        }

        public IActionResult recoveryPassword()
        {
            return View();
        }
    }
}
