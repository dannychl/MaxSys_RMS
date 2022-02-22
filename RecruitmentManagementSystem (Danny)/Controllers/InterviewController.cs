using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using RecruitmentManagementSystem__Danny_.DAL;
using RecruitmentManagementSystem__Danny_.Models;
using System.Net;

namespace RecruitmentManagementSystem__Danny_.Controllers
{
    public class InterviewController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        Interview Interviewer = new Interview();
        InterviewDetail InterviewDetail = new InterviewDetail();
        //Home home = new Home();
        // GET: Interview
        public ActionResult Index()
        {         
            return View(db.Interviewer.ToList());
        }

        // GET: Interview/Details/5
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

        // GET: Interview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Interview/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Interview/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviewer.Find(id);
            ViewBag.intervieweeName = interview.IntervieweeName;
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // POST: Interview/Edit/5
        [HttpPost]
        public ActionResult Edit(DateTime interviewTime, DateTime interviewDate, string selectInterviewer, [Bind(Include = "Id, IntervieweeName, IntervieweeStatus, FirstInterviewStatus, SecondInterviewStatus, IntervieweeResumeLink, CandidateId")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                if (selectInterviewer.Equals("Sky"))
                {
                    db.InterviewDetail.Add(new InterviewDetail
                    {
                        IntervieweeId = interview.Id,
                        InterviewerUserId = 1,
                        InterviewTime = interviewTime,
                        InterviewDate = interviewDate
                    });
                }
                else if(selectInterviewer.Equals("Danny"))
                {
                    db.InterviewDetail.Add(new InterviewDetail
                    {
                        IntervieweeId = interview.Id,
                        InterviewerUserId = 2,
                        InterviewTime = interviewTime,
                        InterviewDate = interviewDate
                    });
                }
                else
                {
                    db.InterviewDetail.Add(new InterviewDetail
                    {
                        IntervieweeId = interview.Id,
                        InterviewerUserId = 1,
                        InterviewTime = interviewTime,
                        InterviewDate = interviewDate
                    });
                    db.InterviewDetail.Add(new InterviewDetail
                    {
                        IntervieweeId = interview.Id,
                        InterviewerUserId = 2,
                        InterviewTime = interviewTime,
                        InterviewDate = interviewDate
                    });
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interview);
        }

        // GET: Interview/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview Interviewer = db.Interviewer.Find(id);
            if (Interviewer == null)
            {
                return HttpNotFound();
            }
            return View(Interviewer);
        }

        // POST: Interview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Interview Interviewer = db.Interviewer.Find(id);
            db.Interviewer.Remove(Interviewer);
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
