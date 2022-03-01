using System;
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
        User Users = new User();
        //Home home = new Home();
        // GET: Interview
        public ActionResult Index(string searchStatus)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Hired", Value = "Hired" });
            items.Add(new SelectListItem { Text = "Reject", Value = "Reject" });
            items.Add(new SelectListItem { Text = "KIV", Value = "KIV" });
            items.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            ViewBag.InterviewStatus = items;
            var model = from s in db.Interviewer select s;
            if (searchStatus == null)
            {
                model = from s in db.Interviewer
                            where s.IntervieweeStatus == "Pending"
                            select s;
            }
            else
            {
                model = from s in db.Interviewer
                        where s.IntervieweeStatus == searchStatus
                        select s;
            }
            return View(model);
            /*return View(db.Interviewer.ToList());*/
        }

        [HttpPost]
        public ActionResult Index(string ddlStatus, [Bind(Include = "Id, IntervieweeId, IntervieweeUserId, InterviewTime, InterviewDate, IntervieweRemarks, InterviewResult, InterviewProgress")] InterviewDetail interview)
        {

            /*List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Hired", Value = "Hired" });
            items.Add(new SelectListItem { Text = "Reject", Value = "Reject" });
            items.Add(new SelectListItem { Text = "KIV", Value = "KIV" });
            ViewBag.InterviewStatus = items;*/
            //InterviewStatus model = InterviewStatusModel(status);
            //return RedirectToAction("Index", new { id = status });

            ViewBag.searchStatus = ddlStatus;
            return RedirectToAction("Index", new { searchStatus = ddlStatus });
        }

        //private static InterviewStatus InterviewStatusModel(string status)
        //{
        //    using (DatabaseContext db = new DatabaseContext())
        //    {
        //        InterviewStatus model = new InterviewStatus()
        //        {
        //            Status = (DbSet<Interview>)(from s in db.Interviewer
        //                      where s.IntervieweeStatus == status
        //                      select s)
        //    };

        //        return model;
        //    }
        //}

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

            if (title == "Hired")
            {
                if (title.Equals("Hired"))
                {
                    //interview = db.Interviewer.Find(id);
                    interview.IntervieweeStatus = "Hired";
                    db.Entry(interview).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (title.Equals("Reject"))
                {
                    //interview = db.Interviewer.Find(id);
                    interview.IntervieweeStatus = "Reject";
                    db.Entry(interview).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (title.Equals("KIV"))
                {
                    //interview = db.Interviewer.Find(id);
                    interview.IntervieweeStatus = "KIV";
                    db.Entry(interview).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
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
                                 select s.InterviewTime).First();

            string interviewDetailId = string.Join(",",(from s in db.InterviewDetail
                                 where s.IntervieweeId == id & s.InterviewProgress == progress
                                 select s.Id).ToArray());      // used to find interviewDetailId's primary key


            ViewBag.InterviewDetailId = interviewDetailId;
            ViewBag.Progress = progress.ToString();

            ViewBag.InterviewDate = interviewDate;
            //ViewBag.InterviewTime = interviewTime;
            ViewBag.InterviewTime = interviewTime;
            ViewBag.num = num;
            return View(interview);
        }

        [HttpPost]
        public ActionResult UpdateInterviewDetails(FormCollection fc, string interviewTime, DateTime interviewDate, string selectInterviewer, [Bind(Include = "Id, IntervieweeId, IntervieweeUserId, InterviewTime, InterviewDate, IntervieweRemarks, InterviewResult, InterviewProgress")] InterviewDetail interview)
        {

            int progress = Int32.Parse(fc["Progress"]);
            string interviewDetailId = fc["InterviewDetailId"];
            string[] interviewDetailIdArray = interviewDetailId.Split(',');
            
            

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
                if (interviewDetailIdArray.Length > 1)
                {
                    for (int i = 0; i < interviewDetailIdArray.Length; i++)
                    {
                        InterviewDetail InterviewDetail = db.InterviewDetail.Find(Int32.Parse(interviewDetailIdArray[i]));
                        db.InterviewDetail.Remove(InterviewDetail);
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
                else
                {
                    InterviewDetail InterviewDetail = db.InterviewDetail.Find(Int32.Parse(interviewDetailIdArray[0]));
                    if (selectInterviewer.Equals("Both")) // 1 interviewer to 2 interviewers
                    {
                        db.InterviewDetail.Remove(InterviewDetail);
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
                    }
                    else // 1 interviewer to 1 interviewer
                    {
                        int interviewerId = (from s in db.User
                                             where s.Username == selectInterviewer
                                             select s.Id).First();
                        InterviewDetail.InterviewerUserId = interviewerId;
                        InterviewDetail.InterviewDate = interviewDate;
                        InterviewDetail.InterviewTime = interviewTime;
                        db.Entry(InterviewDetail).State = EntityState.Modified;
                    }
                    
                }
                /*Interview temp = db.Interviewer.Find(interview.Id);
                interview = temp;*/

                //InterviewDetail interviewDetail = db.InterviewDetail.Find(interviewDetailId);
                

                //interviewDetail.InterviewDate = interviewDate;
                //interviewDetail.InterviewTime = interviewTime;


                // checkSubmit = true when either one of the interviewStatus is TBA
                // checkSubmit = false when either one of the interviewStatus is No
                /* if ((interview.FirstInterviewerStatus.Equals("No") && checkSubmit && btnClicked.Equals("FirstInterview"))
                     || interview.SecondInterviewerStatus.Equals("No") && checkSubmit && btnClicked.Equals("SecondInterview"))
                 { 

                 }
                 else if (interview.SecondInterviewerStatus.Equals("No") && checkSubmit && btnClicked.Equals("FirstInterview"))
                 {
                 }*/

                //db.Entry(interviewDetail).State = EntityState.Modified;
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
