using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVideoteca.Data;
using ProyectoVideoteca.Models;
using ProyectoVideoteca.Models.Domain;
using ProyectoVideoteca.Models.DTO;
using ProyectoVideoteca.Repositories.Abstract;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoVideoteca.Controllers
{
    public class UserAuthenticationController : Controller
    {
        TestUCRContext db = new TestUCRContext();
        private readonly IUserAuthenticationService _service; //database context authentication
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAuthenticationController(IUserAuthenticationService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }

        //for recovery password
        private const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        private static readonly RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public IActionResult Registration()
        {
            return View();
        }

        //user register
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            var user = new tb_USER();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            user.USERNAME = model.UserName;
            user.PASSWORD=model.Password;
            user.EMAIL = model.Email;
            user.NAME = model.Name;
            user.PASSWORD_CONFIRM= model.PasswordConfirm;
            user.ROLE = "user";
            model.Role = "user";

            db.tb_USER.Add(user);
            db.SaveChanges();
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }

       
        public IActionResult Login()
        {
            return View();
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


        public ActionResult recoveryPassword()
        {
            return View();
        }
        
        public async Task sendEmail(string email, string username)
        {
            try
            {
                TestUCRContext db = new TestUCRContext();
                ApplicationUser user = await _userManager.FindByNameAsync(username); //to know the authenticated user
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("qstream2023@gmail.com", "dacgtkkijjtwqknj"); //credentials
                    string newPassword = GenerateRandomPassword();

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user); //generate token
                    var result = await _userManager.ResetPasswordAsync(user, token, newPassword); //update password

                    var userDB=db.tb_USER.Find(username); 
                    userDB.PASSWORD=newPassword;
                    userDB.PASSWORD_CONFIRM = newPassword;
                    db.tb_USER.Update(userDB);
                    db.SaveChanges(); //update password in testUCR

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("qstream2023@gmail.com");
                        mail.To.Add(email);
                        mail.Subject = "Recovery password";
                        mail.Body = "This is your new password: " + newPassword;

                        smtpClient.Send(mail); //send email
                    }
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }

        //method to generate random password
        public string GenerateRandomPassword()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < 8; i++) //password of 8 characters
            {
                password.Append(validChars[random.Next(validChars.Length)]);
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password.ToString()); //string 64bytes
            string newPass = Convert.ToBase64String(passwordBytes); //convert to string

            return newPass;
        }

    }
}
