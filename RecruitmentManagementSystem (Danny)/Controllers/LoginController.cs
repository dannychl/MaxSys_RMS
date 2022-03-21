using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecruitmentManagementSystem__Danny_.DAL;
using RecruitmentManagementSystem__Danny_.Models;

namespace RecruitmentManagementSystem__Danny_.Controllers
{
    public class LoginController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        User user = new User();

        // GET: Login
        public ActionResult Index()
        {
            Session["Id"] = null;
            Session["Roles"] = null;
            Session["Username"] = null;
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.AddHeader("Cache-control", "no-store, must-revalidate, private, no-cache");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.AppendToLog("window.location.reload();");

            return View();
        }

        [HttpPost]
        public ActionResult Index(User loginUser)
        {
            var userLoggedIn = db.User.SingleOrDefault(x => x.Email == loginUser.Email && x.Password == loginUser.Password);

            if(userLoggedIn != null)
            {
                Session["Id"] = userLoggedIn.Id;
                Session["Roles"] = userLoggedIn.Roles;
                Session["Username"] = userLoggedIn.Username;

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.ErrorMessage = "Email or Password is wrong";
                return View();
            }
            
        }
    }
}