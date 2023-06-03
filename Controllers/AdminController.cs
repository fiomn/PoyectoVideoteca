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

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "admin")] //just admin can use this controller
    public class AdminController : Controller
    {
        private TestUCRContext db = new TestUCRContext(); //database context

        private readonly IUserAuthenticationService _service; //database context authentication
        public AdminController(IUserAuthenticationService service)
        {
            this._service = service;
        }

        public ActionResult AdminMain()
        {
            return View();
        }

        public ActionResult Display()
        {
            var userList = new List<tb_USER>();
            userList = db.tb_USER.FromSqlRaw("exec dbo.getUser").ToList();
            return View(userList);
        }

        //GET
        public ActionResult Create()
        {
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
            return View(user);
        }

        //GET: AdminController/Edit/5
  
        public ActionResult Edit(string userName)
        {
            var user = db.tb_USER.Find(userName);
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
        public ActionResult Delete(string userName)
        {
            var person = db.tb_USER.Find(userName);
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

        //download and generate PDF
        //is like an APi
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
    }
}
