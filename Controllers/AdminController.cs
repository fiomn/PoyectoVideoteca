using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoVideoteca.Data;
using ProyectoVideoteca.Models;
using ProyectoVideoteca.Models.Domain;
using System;
using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using ProyectoVideoteca.Models.DTO;
using ProyectoVideoteca.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "admin")] //just admin can use this controller
    public class AdminController : Controller
    {
        //references DataBases
        private TestUCRContext db = new TestUCRContext(); //database context
        private readonly IUserAuthenticationService _service; //database context authentication
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IUserAuthenticationService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }

        //**********************************MOVIES AND SERIES*********************************
        public ActionResult AdminMain()
        {
            Random random = new Random();

            var movies = new List<tb_MOVIE>();

            movies = db.tb_MOVIE.FromSqlRaw(@"exec dbo.GetMovies").ToList();

            var genres = new List<tb_GENRE>();
            genres = db.tb_GENRE.FromSqlRaw(@"exec dbo.GetGenres").ToList();

            List<tb_GENRE> randomGenres = genres.OrderBy(x => random.Next()).ToList();

            tb_MOVIESANDGENRES moviesAndGenres = new tb_MOVIESANDGENRES(movies, randomGenres);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return RedirectToAction("ClientMain", "Client", moviesAndGenres);

        }

        //get actual color of body
        public tb_GLOBALSETTING getMode()
        {
            //get color mode from BD
            var mode = new tb_GLOBALSETTING();
            mode = db.tb_GLOBALSETTING.FromSqlRaw("exec dbo.getMode").AsEnumerable().FirstOrDefault();
            return mode;
        }

        public ActionResult DisplaySeriesAdmin()
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


            return RedirectToAction("DisplaySeries", "Client", seriesAndGenres);
        }

        //When User clicks a movie
        public ActionResult detailsMovies(string TITLE, int? page)
        {
            tb_MOVIE.currentMovie = TITLE;

            var movie = db.tb_MOVIE.FromSqlRaw(@"exec DetailsMovie @TITLE", new SqlParameter("@TITLE", TITLE)).ToList().FirstOrDefault();

            var comments = db.tb_RATING.FromSqlRaw(@"exec GetComments @Title", new SqlParameter("@Title", TITLE)).ToList();

            int itemsPerPage = 10;
            int totalPages = (int)Math.Ceiling((double)comments.Count / 10);
            totalPages = Math.Max(totalPages, 1); // Asegura que haya al menos 1 página

            int currentPage = page ?? 1; // Si no se proporciona el parámetro "page", asume la página 1

            int startIndex = (currentPage - 1) * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, comments.Count);
            var commentsPerPage = comments.GetRange(startIndex, endIndex - startIndex);

            tb_MOVIEANDCOMMENTS movieAndComments = new tb_MOVIEANDCOMMENTS(movie, comments, itemsPerPage, totalPages, currentPage);

            //Mode Visual Types
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return RedirectToAction("detailsMovies", "Client", movieAndComments);
        }

        public ActionResult detailsSeries(string TITLE, int? page)
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

            //Mode Visual Types
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;

            return RedirectToAction("detailsSerie", "Client", serieAndComments);
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

            return RedirectToAction("detailsSerie", "Client", serieAndComments);
        }



        //************************** USERS MANAGEMENT *****************************************

        //get users client
        public ActionResult Display()
        {
            var userList = new List<tb_USER>();
            userList = db.tb_USER.FromSqlRaw("exec dbo.getUser").ToList();
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(userList);
        }

        //GET
        //create user client
        public ActionResult Create()
        {
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View();
        }

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

        //GET DETAILS
        public ActionResult Details(string userName)
        {
            var user = db.tb_USER.FromSqlRaw(@"exec DetailsUser @USERNAME", new SqlParameter("@USERNAME", userName)).ToList().FirstOrDefault();
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(user);
        }

        //GET: AdminController/Edit/5
        //edit user client
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
                var result = await _service.EditAsync(model); //save users in dataBaseContext

                db.tb_USER.Update(user); //save users in testUCR
                db.SaveChanges();
                return RedirectToAction(nameof(Display));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //GET
        //delete user client
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

        //******************************* REPORT *****************************************
        //download and generate PDF
        public IActionResult DownloadPDF()
        {
            var userList = new List<tb_USER>();
            userList = db.tb_USER.FromSqlRaw("exec dbo.getUser").ToList();

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

        // ************************* PROFILE ***************************
        //edit profile user
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

        //edit profile user
        public ActionResult editUser(string userName)
        {
            var user = db.tb_USER.Find(userName);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> editUser(tb_USER user, RegistrationModel model)
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

                    //update profile picture in db
                    var userBD = db.tb_USER.FromSqlRaw(@"exec getUserByName @username", new SqlParameter("@username", user.UserName)).AsEnumerable().FirstOrDefault();
                    userBD.IMG = profilePictureBase64;
                    user.ProfilePicture = profilePictureBase64;

                    //save changes in db
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
            return RedirectToAction(nameof(editUser));
        }

        //change profile picture
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

        //*****************************CRUD MOVIES***************************************

        //get movies
        public ActionResult displayMovies()
        {
            var moviesList = new List<tb_MOVIE>();
            moviesList = db.tb_MOVIE.FromSqlRaw("exec dbo.getMovie").ToList();
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(moviesList);
        }

        //create movies
        public ActionResult createMovies()
        {
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View();
        }

        [HttpPost]
        public IActionResult createMovies(tb_MOVIE movie)
        {
            try
            {
                db.tb_MOVIE.Add(movie); //save movies in db
                db.SaveChanges();
                return RedirectToAction(nameof(displayMovies));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //edit movies
        public ActionResult editMovies(string title)
        {
            var movie = db.tb_MOVIE.Find(title);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> editMovies(tb_MOVIE movie)
        {
            try
            {
                db.tb_MOVIE.Update(movie); //save users in testUCR
                db.SaveChanges();
                return RedirectToAction(nameof(displayMovies));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //delete movies
        public ActionResult deleteMovies(string title)
        {
            var movie = db.tb_MOVIE.Find(title);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(movie);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deleteMovies(tb_MOVIE movie)
        {
            try
            {
                db.tb_MOVIE.Remove(movie);
                db.SaveChanges();
                return RedirectToAction(nameof(displayMovies));
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        //*********************************CRUD SERIES************************************************

        //get series from db
        public ActionResult displaySeries()
        {
            var seriesList = new List<tb_SERIE>();
            seriesList = db.tb_SERIE.FromSqlRaw("exec dbo.GetSeries").ToList();
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(seriesList);
        }

        //create series
        public ActionResult createSeries()
        {
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> createSeries(tb_SERIE serie)
        {
            try
            {
                db.tb_SERIE.Add(serie); //save movies in db
                db.SaveChanges();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //edit series
        public ActionResult editSeries(string title)
        {
            var serie = db.tb_SERIE.Find(title);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(serie);
        }

        [HttpPost]
        public async Task<IActionResult> editSeries(tb_SERIE serie)
        {
            try
            {
                db.tb_SERIE.Update(serie); //save series in db
                db.SaveChanges();
                return RedirectToAction(nameof(displaySeries));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //delete series
        public ActionResult deleteSeries(string title)
        {
            var serie = db.tb_SERIE.Find(title);
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(serie);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deleteSeries(tb_SERIE serie)
        {
            try
            {
                db.tb_SERIE.Remove(serie);
                db.SaveChanges();
                return RedirectToAction(nameof(displaySeries));
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        //****************************** SEASONS ***********************

        //create season of a serie
        public ActionResult createSeason(string TITLE)
        {
            var season = new tb_SEASON();
            season.TITLE = TITLE;
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(season);
        }

        [HttpPost]
        public async Task<IActionResult> createSeason(tb_SEASON season)
        {
            try
            {
                db.tb_SEASON.Add(season); //save season in db
                db.SaveChanges();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //************************** EPISODES ******************

        //create episodes of a season
        public ActionResult createEpisode()
        {
            var episode = new tb_EPISODE();
            var id = db.tb_SEASON.FromSqlRaw(@"exec getID_Season").ToList().FirstOrDefault(); //get id of last season (is fk)
            episode.SEASON_ID = id.SEASON_ID;
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(episode);
        }

        [HttpPost]
        public async Task<IActionResult> createEpisode(tb_EPISODE episode)
        {
            try
            {
                db.tb_EPISODE.Add(episode); //save season in db
                db.SaveChanges();
                return RedirectToAction(nameof(displaySeries));
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        //************** SECONDARY ACTORS SERIES *******************

        //create news secondary actors of series
        public ActionResult createActorsSeries(string TITLE)
        {
            var actor = new tb_SECONDARY_ACTOR();
            actor.SERIE_TITLE = TITLE; //title of serie
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> createActorsSeries(tb_SECONDARY_ACTOR actor)
        {
            try
            {
                db.tb_SECONDARY_ACTOR.Add(actor); //save season in db
                db.SaveChanges();
                return RedirectToAction(nameof(displaySeries));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //************** SECONDARY ACTORS MOVIES *******************

        //create news secondary actors of movies
        public ActionResult createActorsMovies(string TITLE)
        {
            var actor = new tb_SECONDARY_ACTOR();
            actor.MOVIE_TITLE = TITLE; //movie title
            tb_GLOBALSETTING mode = getMode();
            ViewBag.Mode = mode.mode;
            ViewBag.ModeBtn = mode.modeBtn;
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> createActorsMovies(tb_SECONDARY_ACTOR actor)
        {
            try
            {
                db.tb_SECONDARY_ACTOR.Add(actor); //save season in db
                db.SaveChanges();
                return RedirectToAction(nameof(displaySeries));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
