/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecruitmentManagementSystem__Danny_.Models;
using RecruitmentManagementSystem__Danny_.DAL;

namespace RecruitmentManagementSystem__Danny_.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();


        static IList<Home> studentList = new List<Home>{
                new Home() { Id = 1, Name = "John", Age = 18 } ,
                new Home() { Id = 2, Name = "Steve",  Age = 21 } ,
                new Home() { Id = 3, Name = "Bill",  Age = 25 } ,
                new Home() { Id = 4, Name = "Ram" , Age = 20 } ,
                new Home() { Id = 5, Name = "Ron" , Age = 31 } ,
                new Home() { Id = 6, Name = "Chris" , Age = 17 } ,
                new Home() { Id = 7, Name = "Rob" , Age = 19 }
            };
        // GET: Students
        public ActionResult Index()
        {
            //return View(studentList.OrderBy(s => s.Id).ToList());
            return View(db.Candidate);
        }

        public ActionResult Edit(int Id)
        {
            //here, get the student from the database in the real application

            //getting a student from collection for demo purpose
            var std = studentList.Where(s => s.Id == Id).FirstOrDefault();

            return View(std);
        }

        [HttpPost]
        public ActionResult Edit(Home std)
        {
            //update student in DB using EntityFramework in real-life application

            //update list by removing old student and adding updated student for demo purpose
            var student = studentList.Where(s => s.Id == std.Id).FirstOrDefault();
            studentList.Remove(student);
            studentList.Add(std);

            return RedirectToAction("Index");
        }

    }
}*/