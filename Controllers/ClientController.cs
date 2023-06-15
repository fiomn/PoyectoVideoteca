using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProyectoVideoteca.Data;
using ProyectoVideoteca.Models;
using ProyectoVideoteca.Models.Domain;
using ProyectoVideoteca.Models.DTO;
using ProyectoVideoteca.Repositories.Abstract;
using QuestPDF.Helpers;
using System.Diagnostics;

namespace ProyectoVideoteca.Controllers
{

    [Authorize] //everyone can use this controller
    public class ClientController : Controller
    {
        private TestUCRContext db = new TestUCRContext(); //database context
        private readonly IUserAuthenticationService _service; //database context authentication
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(IUserAuthenticationService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }

        //*************************** SERIES AND MOVIES *********************************
        public ActionResult ClientMain()
        {
            var movies = new List<tb_MOVIE>();

            movies = db.tb_MOVIE.FromSqlRaw(@"exec dbo.GetMovies").ToList();

            var genres = new List<tb_GENRE>();
            genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

            tb_MOVIESANDGENRES moviesAndGenres = new tb_MOVIESANDGENRES(movies, genres);
            MoviesList.list = movies;

            return View(moviesAndGenres);
        }

        public ActionResult DisplaySeries()
        {

            var series = new List<tb_SERIE>();

            series = db.tb_SERIE.FromSqlRaw(@"exec dbo.GetSeries").ToList();

            var genres = new List<tb_GENRE>();
            genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

            tb_SERIESANDGENRES seriesAndGenres = new tb_SERIESANDGENRES(series, genres);

            return View(seriesAndGenres);
        }

        //When User clicks a movie
        public ActionResult detailsMovies(string TITLE)
        {
            tb_MOVIE.currentMovie = TITLE;

            var movie = db.tb_MOVIE.FromSqlRaw(@"exec DetailsMovie @TITLE", new SqlParameter("@TITLE", TITLE)).ToList().FirstOrDefault();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", TITLE)).ToList();

            int totalPages = (int)Math.Ceiling((double)comments.Count / 5);

            tb_MOVIEANDCOMMENTS movieAndComments = new tb_MOVIEANDCOMMENTS(movie, comments, totalPages);



            return View("detailsMovies", movieAndComments);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return user;
        }

        [HttpPost]
        public IActionResult userComment(string comment, string score)
        {
            //get the current user
            var user = GetCurrentUserAsync().Result;

            db.tb_RATING.FromSqlRaw(@"exec InsertComment @Title, @Username, @Rating, @Comment",
                new SqlParameter("@Title", tb_MOVIE.currentMovie),
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@Rating", score),
                new SqlParameter("@Comment", comment));
            db.SaveChanges();

            return RedirectToAction("ClientMain");
        }


        public ActionResult detailsSerie(string TITLE)
        {
            var serie = db.tb_SERIE.FromSqlRaw(@"exec DetailsSeries @TITLE", new SqlParameter("@TITLE", TITLE)).ToList().FirstOrDefault();
            return View("detailsSeries", serie);
        }

        //search by name and genre with an APi
        [HttpGet]
        public string search(string inputSearch)
        {
            var movies = new List<tb_MOVIE>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7053/Search/inputSearch");
                var responseTask = client.GetAsync(inputSearch);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<List<tb_MOVIE>>();
                    readTask.Wait();

                    movies = readTask.Result;
                    return JsonConvert.SerializeObject(movies);
                }
                return JsonConvert.SerializeObject(movies);
            }
        }


        //****************************** PROFILE USER **************************
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
                return View(userByName);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult edit(string userName)
        {
            var user = db.tb_USER.Find(userName);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> edit(tb_USER user, RegistrationModel model)
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
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View("ChangeProfilePicture");
                    }
                }
            }
            return RedirectToAction(nameof(edit));
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
            return View();
        }

        
    }
}
