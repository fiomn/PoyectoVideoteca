using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVideoteca.Models.DTO;
using ProyectoVideoteca.Repositories.Abstract;
using System.Net.Mail;
using System.Net;

namespace ProyectoVideoteca.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;
        public UserAuthenticationController(IUserAuthenticationService service)
        {
            this._service = service;
        }

        public IActionResult Registration()
        {
            return View();
        }

        //user register
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Role = "user";
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RecoveryPassword()
        {
            return View();
        }

        
        //recovery password by email
        public void sendEmail(string email)
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _service.LoginAsync(model);
            if ((result.StatusCode == 1) && (model.UserName == "Admin")) //can do it
            {
                return RedirectToAction("AdminMain", "Admin");
            }
            if ((result.StatusCode == 1) && (model.UserName == "SuperAdmin")) //can do it
            {
                return RedirectToAction("SuperAdminMain", "superAdmin");
            }
            if ((result.StatusCode == 1) && (model.UserName != "SuperAdmin") && (model.UserName != "Admin")) //can do it
            {
                return RedirectToAction("ClientMain", "client");
            }
            else
            {
                TempData["msg"] = result.Message; //is like ViewBag message
                return RedirectToAction(nameof(Login)); //can't register user
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //superAdmin register
        public async Task<IActionResult> RegSuperAdmin()
        {
            var model = new RegistrationModel
            {
                UserName = "SuperAdmin",
                Name = "SuperAdministrator",
                Email = "fio.mn1911@gmail.com",
                Password = "Admin2023!",
            };
            model.Role = "superAdmin";
            var result = await _service.RegistrationAsync(model);
            return Ok(result);
        }
    }
}
