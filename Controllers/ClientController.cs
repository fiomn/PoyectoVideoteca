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
using System;
using System.Diagnostics;
using static QuestPDF.Helpers.Colors;

namespace ProyectoVideoteca.Controllers
{

    [Authorize] //everyone can use this controller
    public class ClientController : Controller
    {
        //references DataBases
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
            Random random = new Random();

            var movies = new List<tb_MOVIE>();

            movies = db.tb_MOVIE.FromSqlRaw(@"exec dbo.GetMovies").ToList();

            var genres = new List<tb_GENRE>();
            genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

            List<tb_GENRE> randomGenres = genres.OrderBy(x => random.Next()).ToList();

            tb_MOVIESANDGENRES moviesAndGenres = new tb_MOVIESANDGENRES(movies, randomGenres);
            MoviesList.list = movies;
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View(moviesAndGenres);
        }

        //get current color of body
        public tb_GLOBALSETTING getMode()
        {
            //get color mode from BD
            var mode = new tb_GLOBALSETTING();
            mode = db.tb_GLOBALSETTING.FromSqlRaw("exec dbo.getMode").AsEnumerable().FirstOrDefault();
            return mode;
        }

        //get series
        public ActionResult DisplaySeries()
        {
            Random random = new Random();

            var series = new List<tb_SERIE>();

            series = db.tb_SERIE.FromSqlRaw(@"exec dbo.GetSeries").ToList();

            var genres = new List<tb_GENRE>();
            genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

            List<tb_GENRE> randomGenres = genres.OrderBy(x => random.Next()).ToList();

            tb_SERIESANDGENRES seriesAndGenres = new tb_SERIESANDGENRES(series, randomGenres);

            //Mode Visual Types
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(seriesAndGenres);
        }

        //When User clicks a movie
        public ActionResult detailsMovies(string TITLE, int? page)
        {
            tb_MOVIE.currentMovie = TITLE;

            var movie = db.tb_MOVIE.FromSqlRaw(@"exec DetailsMovie @TITLE", new SqlParameter("@TITLE", tb_MOVIE.currentMovie)).ToList().FirstOrDefault();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", tb_MOVIE.currentMovie)).ToList();

            int itemsPerPage = 10;
            int totalPages = (int)Math.Ceiling((double)comments.Count / 10);
            totalPages = Math.Max(totalPages, 1); // Asegura que haya al menos 1 página

            int currentPage = page ?? 1; // Si no se proporciona el parámetro "page", asume la página 1

            tb_MOVIEANDCOMMENTS movieAndComments = new tb_MOVIEANDCOMMENTS(movie, comments, itemsPerPage, totalPages, currentPage);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View("detailsMovies", movieAndComments);
        }

        public ActionResult ActorsDetails(string TITLE)
        {

            var movie = db.tb_MOVIE.FromSqlRaw(@"exec DetailsMovie @TITLE", new SqlParameter("@TITLE", TITLE)).ToList().FirstOrDefault();

            var secondaryActors = db.tb_SECONDARY_ACTOR.FromSqlRaw(@"exec GetSecondaryActors @Title", new SqlParameter("@Title", TITLE)).ToList();

            tb_ACTOR_DETAILS actors = new tb_ACTOR_DETAILS(movie, secondaryActors);

            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View("ActorsDetails", actors);
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

            float scoreF = float.Parse(score);
            scoreF = (float)Math.Round(scoreF, 1);

            db.Database.ExecuteSqlRaw(@"exec InsertCommentMovie @Title, @Username, @Comment, @Rating",
                new SqlParameter("@Title", tb_MOVIE.currentMovie),
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@Comment", comment),
                new SqlParameter("@Rating", scoreF));

            db.SaveChanges();
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return RedirectToAction("detailsMovies", "Client", new { TITLE = tb_MOVIE.currentMovie });

        }


        public ActionResult detailsSerie(string TITLE, int? page)
        {
            tb_SERIE.currentSerie = TITLE;

            var serie = db.tb_SERIE.FromSqlRaw(@"exec DetailsSeries @TITLE", new SqlParameter("@TITLE", tb_SERIE.currentSerie)).ToList().FirstOrDefault();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", tb_SERIE.currentSerie)).ToList();

            int itemsPerPage = 10;
            int totalPages = (int)Math.Ceiling((double)comments.Count / 10);
            totalPages = Math.Max(totalPages, 1); // Asegura que haya al menos 1 página

            int currentPage = page ?? 1; // Si no se proporciona el parámetro "page", asume la página 1

            int startIndex = (currentPage - 1) * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, comments.Count);
            var commentsPerPage = comments.GetRange(startIndex, endIndex - startIndex);

            tb_SERIEANDCOMMENTS serieAndComments = new tb_SERIEANDCOMMENTS(serie, comments, itemsPerPage, totalPages, currentPage);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View("detailsSerie", serieAndComments);
        }

        public ActionResult getSeasonSerie(string selectedSeason, int? page)
        {
            var serie = db.tb_SERIE.FromSqlRaw(@"exec DetailsSeries @TITLE", new SqlParameter("@TITLE", tb_SERIE.currentSerie)).ToList().FirstOrDefault();

            var season = db.tb_SEASON.FromSqlRaw(@"exec GetSerieSeasons @TITLE, @NUMBER",
                new SqlParameter("@TITLE", serie.TITLE),
                new SqlParameter("@NUMBER", int.Parse(selectedSeason))).ToList().FirstOrDefault();

            var episodes = db.tb_EPISODE.FromSqlRaw(@"exec GetEpisodesSeason @SEASON_ID", new SqlParameter("@SEASON_ID", season.SEASON_ID)).ToList();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", tb_SERIE.currentSerie)).ToList();

            int itemsPerPage = 10;
            int totalPages = (int)Math.Ceiling((double)comments.Count / 10);
            totalPages = Math.Max(totalPages, 1); // Asegura que haya al menos 1 página

            int currentPage = page ?? 1; // Si no se proporciona el parámetro "page", asume la página 1

            int startIndex = (currentPage - 1) * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, comments.Count);
            var commentsPerPage = comments.GetRange(startIndex, endIndex - startIndex);

            tb_SERIEANDCOMMENTS serieAndComments = new tb_SERIEANDCOMMENTS(serie, season, episodes, comments, itemsPerPage, totalPages, currentPage);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View("detailsSerie", serieAndComments);
        }

        [HttpPost]
        public IActionResult userCommentSerie(string comment, string score)
        {
            //get the current user
            var user = GetCurrentUserAsync().Result;

            float scoreF = float.Parse(score);
            scoreF = (float)Math.Round(scoreF, 1);

            db.Database.ExecuteSqlRaw(@"exec InsertCommentSerie @Title, @Username, @Comment, @Rating",
                new SqlParameter("@Title", tb_SERIE.currentSerie),
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@Comment", comment),
                new SqlParameter("@Rating", scoreF));

            db.SaveChanges();
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return RedirectToAction("detailsSerie", "Client", new { TITLE = tb_SERIE.currentSerie });

        }

        public ActionResult ActorsDetailsSerie(string TITLE)
        {

            var serie = db.tb_SERIE.FromSqlRaw(@"exec DetailsSeries @TITLE", new SqlParameter("@TITLE", TITLE)).ToList().FirstOrDefault();

            var secondaryActors = db.tb_SECONDARY_ACTOR.FromSqlRaw(@"exec GetSecondaryActors @Title", new SqlParameter("@Title", TITLE)).ToList();

            tb_ACTOR_DETAILS actors = new tb_ACTOR_DETAILS(serie, secondaryActors);

            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return View("ActorsDetailsSerie", actors);
        }

        //search movies by name and genre with an APi        
        [HttpGet]
        public string search(string inputSearch)
        {
            var movies = new List<tb_MOVIE>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7181/Search/movies/inputSearch"); //APi returns movies by name and genre
                var responseTask = client.GetAsync(inputSearch);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<List<tb_MOVIE>>();
                    readTask.Wait();

                    movies = readTask.Result;

                    //list of filters movies by name and genre
                    return JsonConvert.SerializeObject(movies);
                }
                return JsonConvert.SerializeObject(movies);
            }
        }

        //search series by name and genre with an APi
        [HttpGet]
        public string searchSeries(string busSeries)
        {
            var series = new List<tb_SERIE>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7181/Search/series/busSeries"); //APi returns series by name and genre
                var responseTask = client.GetAsync(busSeries);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<List<tb_SERIE>>();
                    readTask.Wait();

                    series = readTask.Result;

                    //list of filters series by name and genre
                    return JsonConvert.SerializeObject(series);
                }
                return JsonConvert.SerializeObject(series);
            }
        }


        //****************************** PROFILE USER **************************

        //edit profile user
        public async Task<ActionResult> editProfile()
        {
            try
            {
                //get current user
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                string username = user.UserName;
                var userByName = new tb_USER();
                var parameter = new SqlParameter("@username", username);

                //get user from db
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

        public ActionResult edit(string userName)
        {
            var user = db.tb_USER.Find(userName);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
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
            //get current user
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                if (file != null && file.Length > 0)
                {
                    //convert to base64
                    string profilePictureBase64;
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();
                        profilePictureBase64 = Convert.ToBase64String(fileBytes);
                    }

                    // update profile image in user
                    var userBD = db.tb_USER.FromSqlRaw(@"exec getUserByName @username", new SqlParameter("@username", user.UserName)).AsEnumerable().FirstOrDefault();
                    userBD.IMG = profilePictureBase64;
                    user.ProfilePicture = profilePictureBase64;
                    
                    //save changes
                    var result = await _userManager.UpdateAsync(user);
                    db.tb_USER.Update(userBD);
                    db.SaveChanges();

                    //error
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
            //get authenticated user
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                string profilePictureBase64 = user.ProfilePicture;

                //if exists 
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    //convert base64 to valid url
                    string imageFormat = "image/png"; 
                    string profilePictureUrl = $"data:{imageFormat};base64,{profilePictureBase64}";

                    //send image to view
                    ViewBag.ProfilePicture = profilePictureUrl;
                }
                else
                {
                    //default image
                    ViewBag.ProfilePicture = "https://www.kindpng.com/picc/m/24-248253_user-profile-default-image-png-clipart-png-download.png";
                }
            }
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View();
        }

        //****************************** KIDS MODE **************************

        //public async Task<ActionResult> kidsMode()
        //{
        //    Random random = new Random();

        //    var movies = new List<tb_MOVIE>();

        //    movies = db.tb_MOVIE.FromSqlRaw(@"exec dbo.GetMovies").ToList();

        //    var genres = new List<tb_GENRE>();
        //    genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

        //    List<tb_GENRE> randomGenres = genres.OrderBy(x => random.Next()).ToList();

        //    tb_MOVIESANDGENRES moviesAndGenres = new tb_MOVIESANDGENRES(movies, randomGenres);
        //    MoviesList.list = movies;
        //    string mode = getMode();
        //    ViewBag.Mode = mode;

        //    return View(moviesAndGenres);
        //}
    }
}
