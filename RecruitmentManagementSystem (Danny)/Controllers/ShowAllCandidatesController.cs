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
    public class ShowAllCandidatesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        Home Candidate = new Home();
        Interview Interview = new Interview();

        // GET: ShowAllCandidates
        public ActionResult Index()
        {
            return View(db.Candidate.ToList());
        }

        public PartialViewResult SearchUsers(string searchText)
        {

            var model = from s in db.Candidate
                        select s;

            var result = model.Where(a => a.Name.ToLower().Contains(searchText) || a.Position.ToLower().Contains(searchText)).ToList();

            return PartialView("SearchAllCandidate_View", result);
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

            if (home.ResumeLink != null && home.TestAnsLink!=null)
            {
                ViewBag.ResumeLink = home.ResumeLink;
                ViewBag.TestAnsLink = home.TestAnsLink;
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
        public ActionResult Create([Bind(Include = "Id,Name,Age,Position,ExpectedSalary, DateOfBirth, Gender, WorkingExperience, WorkingExperienceRemarks, ResignPeriod, ProgrammingTest, SQLTest, TestRemarks, Status, CurrentSalary, MethodUsed, PhoneNum, ResumeLink, TestAnsLink")] Home home)
        {
            if (ModelState.IsValid)
            {

                int currentAge = home.DateOfBirth.Year;
                int curAge = DateTime.Now.Year;

                int exactAge = curAge - currentAge;

                home.Age = exactAge;
                home.Status = "None";

                if (home.WorkingExperienceRemarks == null || home.WorkingExperienceRemarks == String.Empty)
                {
                    home.WorkingExperienceRemarks = "None";
                }

                else if (home.ResumeLink == null)
                {
                    home.ResumeLink = "None";
                }
                else if (home.TestAnsLink == null)
                {
                    home.TestAnsLink = "None";
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
        public ActionResult Edit([Bind(Include = "Id,Name,Age,Position,ExpectedSalary, DateOfBirth, Gender, WorkingExperience, WorkingExperienceRemarks, ResignPeriod, ProgrammingTest, SQLTest, TestRemarks, Status, CurrentSalary, MethodUsed, PhoneNum, ResumeLink, TestAnsLink")] Home home)
        {
            if (ModelState.IsValid)
            {
                int currentAge = home.DateOfBirth.Year;
                int curAge = DateTime.Now.Year;

                int exactAge = curAge - currentAge;

                home.Age = exactAge;
                db.Entry(home).State = EntityState.Modified;
                if (home.Status == "Accept" && home.ProgrammingTest != 0 && home.SQLTest != 0)
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
            Home home = db.Candidate.Find(id);
            db.Candidate.Remove(home);
            db.SaveChanges();
            return RedirectToAction("Index");
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Candidate.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);*/
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
