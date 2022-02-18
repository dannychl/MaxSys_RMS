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
    public class CandidatesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        Home Candidate = new Home();
        Interview Interview = new Interview();

        // GET: Candidates
        public ActionResult Index()
        {
            var model = from s in db.Candidate
                        where s.Status == "None"
                        select s;

            return View(model);
        }

        // GET: Candidates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Candidate.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        public ActionResult ShowAllCandidates()
        {
            return View(db.Candidate.ToList());
        }

        // GET: Candidates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,Position,ExpectedSalary, DateOfBirth, Gender, WorkingExperience, WorkingExperienceRemarks, ResignPeriod, ProgrammingTest, SQLTest, TestRemarks, Status, CurrentSalary, MethodUsed, PhoneNum")] Home home)
        {
            if (ModelState.IsValid)
            {
             
                int currentAge = home.DateOfBirth.Year;
                int curAge = DateTime.Now.Year;

                int exactAge = curAge - currentAge;

                home.Age = exactAge;
                home.Status = "None";
                
                if (home.WorkingExperienceRemarks == null  || home.WorkingExperienceRemarks == String.Empty)
                {
                    home.WorkingExperienceRemarks = "None";
                }
                home.TestRemarks = "None";
                db.Candidate.Add(home);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(home);
        }

        // GET: Candidates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Candidate.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,Position,ExpectedSalary, DateOfBirth, Gender, WorkingExperience, WorkingExperienceRemarks, ResignPeriod, ProgrammingTest, SQLTest, TestRemarks, Status, CurrentSalary, MethodUsed, PhoneNum")] Home home)
        {
            if (ModelState.IsValid)
            {


                int currentAge = home.DateOfBirth.Year;
                int curAge = DateTime.Now.Year;

                int exactAge = curAge - currentAge;

                home.Age = exactAge;
                db.Entry(home).State = EntityState.Modified;
                if(home.Status == "Accept")
                {
                    if (!db.Interviewer.Where(u => u.CandidatesId == home.Id).Any())
                    {
                        db.Interviewer.Add(new Interview
                        {
                            IntervieweeName = home.Name,
                            IntervieweeStatus = "Pending",
                            FirstInterviewerStatus = "TBA",
                            SecondInterviewerStatus = "TBA",
                            IntervieweeResumeLink = "None",
                            CandidatesId = home.Id
                        });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(home);
        }

        // GET: Candidates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Candidate.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Home home = db.Candidate.Find(id);
            db.Candidate.Remove(home);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
