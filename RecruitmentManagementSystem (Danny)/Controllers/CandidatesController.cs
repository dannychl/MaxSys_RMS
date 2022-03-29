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
using PagedList.Mvc;
using PagedList;

namespace RecruitmentManagementSystem__Danny_.Controllers
{
    public class CandidatesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        Candidate Candidate = new Candidate();
        Interview Interview = new Interview();

        public PartialViewResult SearchUsers(string searchText, int? page)
        {
          
            var model = from s in db.Candidate
                        where s.Status == "None" | s.ProgrammingTest == 0 | s.SQLTest == 0
                        select s;

            var result = model.Where(a => a.Name.ToLower().Contains(searchText) || a.Position.ToLower().Contains(searchText)).ToList().ToPagedList(page ?? 1, 6);

            return PartialView("SearchCandidate_View", result);
        }

        // GET: Candidates
        public ActionResult Index(int? page)
        {
            if(Session["Username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = from s in db.Candidate
                        where s.Status == "None"
                        select s;

            return View(model.ToList().ToPagedList(page ?? 1, 6));
            /*return View(this.GetCustomers(1));*/
        } 

        // GET: Candidates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate home = db.Candidate.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            if (home.ResumeLink != null && home.ResumeLink != "None")
            {
                ViewBag.ResumeLink = home.ResumeLink;
            }
            if (home.TestAnsLink != null && home.TestAnsLink != "None")
            {
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
        public ActionResult Create([Bind(Include = "Id,Name,Age,Position,ExpectedSalary, DateOfBirth, Gender, WorkingExperience, WorkingExperienceRemarks, ResignPeriod, ProgrammingTest, SQLTest, TestRemarks, Status, CurrentSalary, MethodUsed, PhoneNum, ResumeLink, TestAnsLink, DateCreated")] Candidate home)
        {
            if (ModelState.IsValid)
            {

                home.DateCreated = DateTime.Now;
                int currentAge = home.DateOfBirth.Year;
                int curAge = DateTime.Now.Year;

                int exactAge = curAge - currentAge;

                home.Age = exactAge;
                home.Status = "None";
                
                if (home.WorkingExperienceRemarks == null  || home.WorkingExperienceRemarks == String.Empty)
                {
                    home.WorkingExperienceRemarks = "None";
                }

                if (home.ResumeLink == null || home.ResumeLink == String.Empty)
                {
                    home.ResumeLink = "None";
                }
                if(home.TestAnsLink == null || home.TestAnsLink == String.Empty)
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
            Candidate home = db.Candidate.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,Name,Age,Position,ExpectedSalary, DateOfBirth, Gender, WorkingExperience, WorkingExperienceRemarks, ResignPeriod, ProgrammingTest, SQLTest, TestRemarks, Status, CurrentSalary, MethodUsed, PhoneNum, ResumeLink, TestAnsLink")] Candidate home)
        {
            if (ModelState.IsValid)
            {
                int currentAge = home.DateOfBirth.Year;
                int curAge = DateTime.Now.Year;

                int exactAge = curAge - currentAge;

                home.Age = exactAge;
                if(home.Status == "Accept" && home.ProgrammingTest!=0 && home.SQLTest!=0)
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
                if (home.ResumeLink == null || home.ResumeLink == String.Empty)
                {
                    home.ResumeLink = "None";
                }
                if (home.TestAnsLink == null || home.TestAnsLink == String.Empty)
                {
                    home.TestAnsLink = "None";
                }
                db.Entry(home).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(home);
        }

        // GET: Candidates/Delete/5
        public ActionResult Delete(int? id)
        {
            Candidate home = db.Candidate.Find(id);
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
            Candidate home = db.Candidate.Find(id);
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
