using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProyectoVideoteca.Data;
using ProyectoVideoteca.Models;
using ProyectoVideoteca.Models.Domain;
using ProyectoVideoteca.Models.DTO;
using ProyectoVideoteca.Repositories.Abstract;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Text.RegularExpressions;

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "superAdmin")] //just superAdmin can use this controller
    public class SuperAdminController : Controller
    {
        //references DataBases
        private TestUCRContext db = new TestUCRContext(); //database context
        private readonly IUserAuthenticationService _service; //database context authentication
        private readonly UserManager<ApplicationUser> _userManager;

        public SuperAdminController(IUserAuthenticationService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }

        //*****************Movies and series**************************
        public ActionResult SuperAdminMain()
        {
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return RedirectToAction("ClientMain", "Client");
        }

        //get color of body from dataBase
        public tb_GLOBALSETTING getMode()
        {
            //get color mode from BD
            var mode = new tb_GLOBALSETTING();
            mode = db.tb_GLOBALSETTING.FromSqlRaw("exec dbo.getMode").AsEnumerable().FirstOrDefault();
            return mode;
        }

        //*********************************USERS***********************************
        //display users
        public ActionResult Display()
        {
            var userList = new List<tb_USER>();
            userList = db.tb_USER.FromSqlRaw("exec dbo.GetAdmins").ToList(); //get users admins
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(userList);
        }

        //GET
        public ActionResult Create()
        {
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View();
        }

        //POST
        //create new admin
        [HttpPost]
        public async Task<IActionResult> Create(tb_USER user, RegistrationModel model)
        {
            try
            {
                var result = await _service.RegistrationAsync(model); //save users in dataBaseContext

                db.tb_USER.Add(user); //save users in testUCR
                db.SaveChanges();
                TempData["Message"] = "User was Created Successfully";
                TempData["MessageClass"] = "alert alert-info";
                return RedirectToAction(nameof(Display));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Edit(string userName)
        {
            var user = db.tb_USER.Find(userName);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(user);
        }

        //update admin 
        [HttpPost]
        public async Task<IActionResult> Edit(tb_USER user, RegistrationModel model)
        {

            try
            {
                var result = await _service.EditAsync(model); //save users in dataBaseContext

                string pattern = "^[a-zA-Z ]+$"; //Validation

                if (Regex.IsMatch(user.NAME, pattern))
                {
                    TempData["Message"] = "User was Edited Successfully";
                    TempData["MessageClass"] = "alert alert-info";

                    db.tb_USER.Update(user); //save users in testUCR
                    db.SaveChanges();
                    return RedirectToAction(nameof(editProfile));
                }
                else
                {
                    TempData["Message"] = "Incorrect type file. Verify that you are introducing your information correctly";
                    TempData["MessageClass"] = "alert alert-info";

                    return RedirectToAction("Edit", "Admin", user.USERNAME);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //details admin
        public ActionResult Details(string userName)
        {
            var user = db.tb_USER.FromSqlRaw(@"exec DetailsUser @USERNAME", new SqlParameter("@USERNAME", userName)).ToList().FirstOrDefault();
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(user);
        }

        //GET
        public ActionResult Delete(string userName)
        {
            var person = db.tb_USER.Find(userName);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(person);
        }

        // POST
        //delete admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(tb_USER user, RegistrationModel model)
        {
            try
            {
                var result = await _service.RemoveAsync(model);

                db.tb_USER.Remove(user);
                db.SaveChanges();
                TempData["Message"] = "User was Deleted Successfully";
                TempData["MessageClass"] = "alert alert-info";
                return RedirectToAction(nameof(Display));
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        //******************************* PDF **************************************

        //download and generate PDF
        public IActionResult DownloadPDF()
        {
            var userList = new List<tb_USER>();
            userList = db.tb_USER.FromSqlRaw("exec dbo.GetAdmins").ToList();

            var documentpdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    /*page.Header().Height(100).Background(Colors.Blue.Medium);
                    page.Content().Background(Colors.Grey.Medium);
                    page.Footer().Height(100).Background(Colors.LightBlue.Medium);*/

                    /* page.Header().Row(row =>
                     {
                         row.RelativeItem().Border(1).Background(Colors.Blue.Medium).Height(100);
                         row.RelativeItem().Border(1).Background(Colors.Green.Medium).Height(100);
                         row.RelativeItem().Border(1).Background(Colors.Brown.Medium).Height(100);
                     }); //para hacer filas, relative item divide en 3 porque tenemos 3*/

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });//columns headers

                            table.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("UserName").FontColor("#fff").FontSize(13);
                                header.Cell().Background("#257272").Padding(2).Text("Name").FontColor("#fff").FontSize(13);
                                header.Cell().Background("#257272").Padding(2).Text("Email").FontColor("#fff").FontSize(13);
                                header.Cell().Background("#257272").Padding(2).Text("Role").FontColor("#fff").FontSize(13);
                            });

                            //save info in pdf
                            //borderBottom defines a border
                            foreach (var user in userList)
                            {
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Padding(2).Text(user.USERNAME).FontColor("#000").FontSize(10);
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Padding(2).Text(user.NAME).FontColor("#000").FontSize(10);
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Padding(2).Text(user.EMAIL).FontColor("#000").FontSize(10);
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Padding(2).Text(user.ROLE).FontColor("#000").FontSize(10);
                            }
                        });
                    });

                });
            }).GeneratePdf(); //returns pdf

            var stream = new MemoryStream(documentpdf); //save pdf in memory
            return File(stream, "application/pdf", "UsersList.pdf"); //name and type of pdf
        }

        //****************************** PROFILE ************************************************

        //edit profile users
        public async Task<ActionResult> editProfile()
        {
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                string username = user.UserName;
                var userByName = new tb_USER();
                var parameter = new SqlParameter("@username", username);

                userByName = db.tb_USER.FromSqlRaw(@"exec getUserByName @username", new SqlParameter("@username", username)).AsEnumerable().FirstOrDefault();

                tb_GLOBALSETTING mode = getMode();
                ViewBag.Mode = mode.mode;
                ViewBag.ModeBtn = mode.modeBtn;
                return View(userByName);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //update profile image of users
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            //get actual user
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                if (file != null && file.Length > 0)
                {
                    //convert image to base64
                    string profilePictureBase64;
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();
                        profilePictureBase64 = Convert.ToBase64String(fileBytes);
                    }

                    //update image in user
                    var userBD = db.tb_USER.FromSqlRaw(@"exec getUserByName @username", new SqlParameter("@username", user.UserName)).AsEnumerable().FirstOrDefault();
                    userBD.IMG = profilePictureBase64;
                    user.ProfilePicture = profilePictureBase64;

                    //save in DB
                    var result = await _userManager.UpdateAsync(user);
                    db.tb_USER.Update(userBD);
                    db.SaveChanges();
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View("ChangeProfilePicture");
                    }
                }
            }
            return RedirectToAction(nameof(Edit));
        }

        //method for change profile picture
        public async Task<IActionResult> ChangeProfilePicture()
        {
            //get authenticated user
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                string profilePictureBase64 = user.ProfilePicture;

                //verify that image exists
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    //Convert base64 to valid url
                    string imageFormat = "image/png";
                    string profilePictureUrl = $"data:{imageFormat};base64,{profilePictureBase64}";

                    //send image to view
                    ViewBag.ProfilePicture = profilePictureUrl;
                }
                else
                {
                    //default image if 
                    ViewBag.ProfilePicture = "https://www.kindpng.com/picc/m/24-248253_user-profile-default-image-png-clipart-png-download.png";
                }
            }
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View();
        }

        //**************** GLOBAL SETTINGS *******************

        //save mode of color theme
        public void saveMode(string mode, string modeBtn)
        {
            var setting = new tb_GLOBALSETTING();
            if (mode != null)
            {
                setting.mode = mode; //mode of body color
                setting.modeBtn = modeBtn; //mode of button switch
                db.tb_GLOBALSETTING.Add(setting);
                db.SaveChanges();
            }
        }

    }
}
