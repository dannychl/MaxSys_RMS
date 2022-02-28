﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using RecruitmentManagementSystem__Danny_.DAL;
using RecruitmentManagementSystem__Danny_.Models;
using System.Net;
using System.Data.SqlClient;

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
        public ActionResult Edit(int? id, string title)
        {
            ViewBag.Message = title;
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

            if (title == "FirstInterview" && interview.FirstInterviewerStatus == "No")
            {
                return RedirectToAction("UpdateInterviewDetails", new { id = id , title = title , progress = 1});
            }
            else if (title == "SecondInterview" && interview.SecondInterviewerStatus == "No")
            {
                return RedirectToAction("UpdateInterviewDetails", new { id = id, title = title , progress = 2});
            }

            if (title == "FirstInterview" && interview.FirstInterviewerStatus == "TBA")
            {
                return View(interview);
            }
            else if (title == "SecondInterview" && interview.SecondInterviewerStatus == "TBA")
            {
                return View(interview);
            }

            return View(interview);
            //return PartialView("_EditInterviewDetails", interview);
        }

        public ActionResult UpdateInterviewDetails(int? id, string title, int? progress)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviewer.Find(id);

            /*List<InterviewDetail> data  = db.InterviewDetail.Where(c => c.IntervieweeId == id & c.InterviewProgress == progress).ToList();*/

            ViewBag.intervieweeName = interview.IntervieweeName;
            if (id == null || title == null || progress == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.message = title;
            ViewBag.id = id;

            var num = (from s in db.InterviewDetail
                       where s.IntervieweeId == id & s.InterviewProgress == progress
                       select s).Count();       // used to calculate how many rows of same interviewee id found in table

            if (num == 1) 
            {
                var interviewerId = (from s in db.InterviewDetail
                                     where s.IntervieweeId == id & s.InterviewProgress == progress
                                     select s.InterviewerUserId).First();
                ViewBag.interviewer = interviewerId; // used to find which interviewer to display in drop-down list
            }

            var interviewDate = (from s in db.InterviewDetail
                                 where s.IntervieweeId == id & s.InterviewProgress == progress
                                 select s.InterviewDate).First();
            
            var interviewTime = (from s in db.InterviewDetail
                                 where s.IntervieweeId == id & s.InterviewProgress == progress
                                 select s.InterviewTime);
            

            var interviewDetailId = (from s in db.InterviewDetail
                                 where s.IntervieweeId == id & s.InterviewProgress == progress
                                 select s.Id).First();      // used to find interviewDetailId's primary key


            ViewBag.InterviewDetailId = interviewDetailId;

            ViewBag.InterviewDate = interviewDate;
            //ViewBag.InterviewTime = interviewTime;
            ViewBag.InterviewTime = interviewTime;
            ViewBag.num = num;
            return View(interview);
        }

        [HttpPost]
        public ActionResult UpdateInterviewDetails(FormCollection fc, string interviewTime, DateTime interviewDate, string selectInterviewer, [Bind(Include = "Id, IntervieweeId, IntervieweeUserId, InterviewTime, InterviewDate, IntervieweRemarks, InterviewResult, InterviewProgress")] InterviewDetail interview)
        {
           
            int interviewDetailId = Int32.Parse(fc["InterviewDetailId"]);

           /* if (Request.Form["submit"] != null)
            {
                checkSubmit = true;
            }
            else if (Request.Form["process"] != null)
            {
                checkSubmit = false;
            }*/

            if (ModelState.IsValid)
            {

                /*Interview temp = db.Interviewer.Find(interview.Id);
                interview = temp;*/

                InterviewDetail interviewDetail = db.InterviewDetail.Find(interviewDetailId);


                interviewDetail.InterviewDate = interviewDate;
                interviewDetail.InterviewTime = interviewTime;


                // checkSubmit = true when either one of the interviewStatus is TBA
                // checkSubmit = false when either one of the interviewStatus is No
                /* if ((interview.FirstInterviewerStatus.Equals("No") && checkSubmit && btnClicked.Equals("FirstInterview"))
                     || interview.SecondInterviewerStatus.Equals("No") && checkSubmit && btnClicked.Equals("SecondInterview"))
                 { 

                 }
                 else if (interview.SecondInterviewerStatus.Equals("No") && checkSubmit && btnClicked.Equals("FirstInterview"))
                 {
                 }*/

                db.Entry(interviewDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interview);
        }

            // POST: Interview/Edit/5
            [HttpPost]
        public ActionResult Edit(FormCollection fc, string interviewTime, DateTime interviewDate, string selectInterviewer, [Bind(Include = "Id, IntervieweeName, IntervieweeStatus, FirstInterviewStatus, SecondInterviewStatus, IntervieweeResumeLink, CandidateId")] Interview interview)
        {
            bool checkSubmit = false;

            string btnClicked = fc["Message"];

            if (Request.Form["submit"] != null)
            {
                checkSubmit = true;
            }
            else if (Request.Form["process"]!=null)
            {
                checkSubmit = false;
            }

            if (ModelState.IsValid)
            {
                int progress = 0;
                Interview temp = db.Interviewer.Find(interview.Id);
                interview = temp;
                // checkSubmit = true when either one of the interviewStatus is TBA
                // checkSubmit = false when either one of the interviewStatus is No
                if ((interview.FirstInterviewerStatus.Equals("TBA") && checkSubmit && btnClicked.Equals("FirstInterview"))
                    || (interview.SecondInterviewerStatus.Equals("TBA") && checkSubmit && btnClicked.Equals("SecondInterview")))
                {

                    if (interview.FirstInterviewerStatus.Equals("TBA") && interview.SecondInterviewerStatus.Equals("TBA"))
                    {
                        progress = 1;
                        interview.FirstInterviewerStatus = "No";
                    }
                    else if (interview.FirstInterviewerStatus.Equals("No") && interview.SecondInterviewerStatus.Equals("TBA"))
                    {
                        progress = 2;
                        interview.SecondInterviewerStatus = "No";
                    }

                    switch (selectInterviewer)
                    {
                        case "Sky":
                            {
                                db.InterviewDetail.Add(new InterviewDetail
                                {
                                    IntervieweeId = interview.Id,
                                    InterviewerUserId = 1,
                                    InterviewTime = interviewTime.ToString(),
                                    InterviewDate = interviewDate,
                                    InterviewProgress = progress
                                });
                                break;
                            }
                        case "Danny":
                            {
                                db.InterviewDetail.Add(new InterviewDetail
                                {
                                    IntervieweeId = interview.Id,
                                    InterviewerUserId = 2,
                                    InterviewTime = interviewTime.ToString(),
                                    InterviewDate = interviewDate,
                                    InterviewProgress = progress
                                });
                                break;
                            }
                        case "Both":
                            {
                                db.InterviewDetail.Add(new InterviewDetail
                                {
                                    IntervieweeId = interview.Id,
                                    InterviewerUserId = 1,
                                    InterviewTime = interviewTime.ToString(),
                                    InterviewDate = interviewDate,
                                    InterviewProgress = progress
                                });
                                db.InterviewDetail.Add(new InterviewDetail
                                {
                                    IntervieweeId = interview.Id,
                                    InterviewerUserId = 2,
                                    InterviewTime = interviewTime.ToString(),
                                    InterviewDate = interviewDate,
                                    InterviewProgress = progress
                                });
                                break;
                            }
                    }
                }
                else if (interview.SecondInterviewerStatus.Equals("No") && checkSubmit && btnClicked.Equals("FirstInterview"))
                {
                    
                }

                db.Entry(interview).State = EntityState.Modified;
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
