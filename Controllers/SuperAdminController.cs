using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVideoteca.Data;
using ProyectoVideoteca.Models;
using System;
using System.Data;

namespace ProyectoVideoteca.Controllers
{
    [Authorize(Roles = "superAdmin")] //just superAdmin can use this controller
    public class SuperAdminController : Controller
    {
        private TestUCRContext db = new TestUCRContext();
        public ActionResult SuperAdminMain()
        {
            var userList = new List<tb_USER>();
            using (var dbContext = new TestUCRContext())
            {
                userList = dbContext.tb_USERs.ToList();
            }
            return View(userList);
        }
    }
}
