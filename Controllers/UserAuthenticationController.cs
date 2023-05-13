using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVideoteca.Models.DTO;
using ProyectoVideoteca.Repositories.Abstract;

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

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _service.LoginAsync(model);
            if (result.StatusCode == 1) //can do it
            {
                return RedirectToAction("Index", "Home");
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

        //Admin register
        public async Task<IActionResult> RegAdmin()
        {
            var model = new RegistrationModel
            {
                UserName = "Admin",
                Name = "Administrator",
                Email = "fio.mn1911@gmail.com",
                Password = "Admin2023!",
            };
            model.Role = "admin";
            var result = await _service.RegistrationAsync(model);
            return Ok(result);
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
