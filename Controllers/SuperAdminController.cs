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

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "superAdmin")] //just superAdmin can use this controller
    public class SuperAdminController : Controller
    {
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
            var movies = new List<tb_MOVIE>();

            movies = db.tb_MOVIE.FromSqlRaw(@"exec dbo.GetMovies").ToList();

            var genres = new List<tb_GENRE>();
            genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

            tb_MOVIESANDGENRES moviesAndGenres = new tb_MOVIESANDGENRES(movies, genres);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View(moviesAndGenres);
        }

        public tb_GLOBALSETTING getMode()
        {
            //get color mode from BD
            var mode = new tb_GLOBALSETTING();
            mode = db.tb_GLOBALSETTING.FromSqlRaw("exec dbo.getMode").AsEnumerable().FirstOrDefault();
            return mode;
        }


        public ActionResult DisplaySeriesSuperAdmin()
        {

            var series = new List<tb_SERIE>();

            series = db.tb_SERIE.FromSqlRaw(@"exec dbo.GetSeries").ToList();

            var genres = new List<tb_GENRE>();
            genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

            tb_SERIESANDGENRES seriesAndGenres = new tb_SERIESANDGENRES(series, genres);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View(seriesAndGenres);
        }

        public ActionResult detailsMovies(string TITLE, int? page)
        {
            tb_MOVIE.currentMovie = TITLE;

            var movie = db.tb_MOVIE.FromSqlRaw(@"exec DetailsMovie @TITLE", new SqlParameter("@TITLE", TITLE)).ToList().FirstOrDefault();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", TITLE)).ToList();

            int totalPages = (int)Math.Ceiling((double)comments.Count / 3);

            int currentPage = page ?? 1; // Si no se proporciona el parámetro "page", asume la página 1

            tb_MOVIEANDCOMMENTS movieAndComments = new tb_MOVIEANDCOMMENTS(movie, comments, totalPages, currentPage);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;


            return View("detailsMovies", movieAndComments);
        }

        public ActionResult detailsSeries(string TITLE, int? page)
        {
            tb_SERIE.currentSerie = TITLE;

            var serie = db.tb_SERIE.FromSqlRaw(@"exec DetailsSeries @TITLE", new SqlParameter("@TITLE", tb_SERIE.currentSerie)).ToList().FirstOrDefault();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", tb_SERIE.currentSerie)).ToList();

            int totalPages = (int)Math.Ceiling((double)comments.Count / 3);

            int currentPage = page ?? 1; // Si no se proporciona el parámetro "page", asume la página 1

            tb_SERIEANDCOMMENTS serieAndComments = new tb_SERIEANDCOMMENTS(serie, comments, totalPages, currentPage);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View("detailsSeries", serieAndComments);
        }

        public ActionResult getSeasonSerie(string selectedSeason, int? page)
        {
            var serie = db.tb_SERIE.FromSqlRaw(@"exec DetailsSeries @TITLE", new SqlParameter("@TITLE", tb_SERIE.currentSerie)).ToList().FirstOrDefault();

            var season = db.tb_SEASON.FromSqlRaw(@"exec GetSerieSeasons @TITLE, @NUMBER",
                new SqlParameter("@TITLE", serie.TITLE),
                new SqlParameter("@NUMBER", int.Parse(selectedSeason))).ToList().FirstOrDefault();

            var episodes = db.tb_EPISODE.FromSqlRaw(@"exec GetEpisodesSeason @SEASON_ID", new SqlParameter("@SEASON_ID", season.SEASON_ID)).ToList();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", tb_SERIE.currentSerie)).ToList();

            int totalPages = (int)Math.Ceiling((double)comments.Count / 3);

            int currentPage = page ?? 1; // Si no se proporciona el parámetro "page", asume la página 1

            tb_SERIEANDCOMMENTS serieAndComments = new tb_SERIEANDCOMMENTS(serie, season, episodes, comments, totalPages, currentPage);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View("detailsSeries", serieAndComments);
        }

        //*********************************USERS***********************************
        //display users
        public ActionResult Display()
        {
            var userList = new List<tb_USER>();
            userList = db.tb_USER.FromSqlRaw("exec dbo.GetAdmins").ToList();
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
        [HttpPost]
        public async Task<IActionResult> Create(tb_USER user, RegistrationModel model)
        {
            try
            {
                var result = await _service.RegistrationAsync(model); //save users in dataBaseContext

                db.tb_USER.Add(user); //save users in testUCR
                db.SaveChanges();
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

        [HttpPost]
        public async Task<IActionResult> Edit(tb_USER user, RegistrationModel model)
        {
            try
            {
                var result = await _service.EditAsync(model); //save in context auth
                db.tb_USER.Update(user); //save users in testUCR
                db.SaveChanges();
                return RedirectToAction(nameof(editProfile));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(tb_USER user, RegistrationModel model)
        {
            try
            {
                var result = await _service.RemoveAsync(model);

                db.tb_USER.Remove(user);
                db.SaveChanges();
                return RedirectToAction(nameof(Display));
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        //******************************* PDF **************************************
        //download and generate PDF
        //is like an APi
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
        public async Task<ActionResult> editProfile()
        {
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                string username = user.UserName;
                var userByName = new tb_USER();
                var parameter = new SqlParameter("@username", username);

                userByName = db.tb_USER.FromSqlRaw(@"exec getUserByName @username", new SqlParameter("@username", username)).AsEnumerable().FirstOrDefault();
                ManagementUsers.users = userByName;
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


        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            // Obtén el usuario actual
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                // Verifica si se seleccionó un archivo
                if (file != null && file.Length > 0)
                {
                    // Convierte el archivo en una cadena base64
                    string profilePictureBase64;
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();
                        profilePictureBase64 = Convert.ToBase64String(fileBytes);
                    }

                    // Actualiza la imagen de perfil en el modelo del usuario
                    var userBD = db.tb_USER.FromSqlRaw(@"exec getUserByName @username", new SqlParameter("@username", user.UserName)).AsEnumerable().FirstOrDefault();
                    userBD.IMG = profilePictureBase64;
                    user.ProfilePicture = profilePictureBase64;

                    // Guarda los cambios en la base de datos
                    var result = await _userManager.UpdateAsync(user);
                    db.tb_USER.Update(userBD);
                    db.SaveChanges();
                    if (!result.Succeeded)
                    {
                        // El cambio de correo electrónico no fue exitoso, agrega errores de validación.
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

        public async Task<IActionResult> ChangeProfilePicture()
        {
            // Obtener el usuario actualmente autenticado
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                string profilePictureBase64 = user.ProfilePicture;
                // Verificar si la imagen de perfil existe
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    // Convertir la cadena base64 en una URL válida
                    string imageFormat = "image/png"; // Cambia esto según el formato de imagen que estés utilizando
                    string profilePictureUrl = $"data:{imageFormat};base64,{profilePictureBase64}";
                    // Pasar la URL de la imagen de perfil a la vista
                    ViewBag.ProfilePicture = profilePictureUrl;
                }
                else
                {
                    ViewBag.ProfilePicture = "https://www.kindpng.com/picc/m/24-248253_user-profile-default-image-png-clipart-png-download.png";
                }
            }
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View();
        }

        //**************** GLOBAL SETTINGS *******************
        public void saveMode(string mode, string modeBtn)
        {
            var setting = new tb_GLOBALSETTING();
            if (mode != null)
            {
                setting.mode = mode;
                setting.modeBtn = modeBtn;
                db.tb_GLOBALSETTING.Add(setting);
                db.SaveChanges();
            }
        }

    }
}
