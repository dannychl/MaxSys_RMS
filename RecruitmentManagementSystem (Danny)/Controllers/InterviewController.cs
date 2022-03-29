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

        public PartialViewResult SearchUsers(string searchText, string searchStatus)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Hired", Value = "Hired" });
            items.Add(new SelectListItem { Text = "Reject", Value = "Reject" });
            items.Add(new SelectListItem { Text = "KIV", Value = "KIV" });
            items.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            ViewBag.InterviewStatus = items;
            ViewBag.searchStatus = searchStatus;

            var model = from s in db.Interviewer
                        where s.IntervieweeStatus == "Pending"
                        select s;

            if (searchText == null && searchStatus != null)
            {
                model = from s in db.Interviewer
                        where s.IntervieweeStatus == searchStatus
                        select s;
                return PartialView("Search_GridView", model);
            }
            else if (searchStatus == null && searchText != null)
            {
                model = from s in db.Interviewer
                        where s.IntervieweeStatus == "Pending" && s.IntervieweeName.ToLower().Contains(searchText)
                        select s;
                return PartialView("Search_GridView", model);
            }
            else if (searchStatus == "Hired" || searchStatus == "Reject" || searchStatus == "KIV")
            {
                model = from s in db.Interviewer
                        where s.IntervieweeStatus == searchStatus && s.IntervieweeName.ToLower().Contains(searchText)
                        select s;
                return PartialView("Search_GridView", model);
            }

            var result = model.Where(a => a.IntervieweeName.ToLower().Contains(searchText)).ToList();

            return PartialView("Search_GridView", result);
        }
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
                ViewBag.searchStatus = searchStatus;
                model = from s in db.Interviewer
                        where s.IntervieweeStatus == searchStatus
                        select s;
            }

            /* var result = model.Where(a => a.IntervieweeName.ToLower().Contains(searchText));
             return PartialView("Search_GridView", result);*/
            return View(model);
            /*return View(db.Interviewer.ToList());*/
        }

        [HttpPost]
        public ActionResult Index(string ddlStatus, [Bind(Include = "Id, IntervieweeId, IntervieweeUserId, InterviewTime, InterviewDate, IntervieweRemarks, InterviewResult, InterviewProgress")] InterviewDetail interview)
        {
            return RedirectToAction("Index", new { searchStatus = ddlStatus });
        }

        // GET: Interview/Details/5
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
            /*int candidateId;*/
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviewer.Find(id);
            ViewBag.intervieweeName = interview.IntervieweeName;
            ViewBag.Message = title;
            ViewBag.intervieweeId = interview.CandidatesId;

            if (interview == null)
            {
                return HttpNotFound();
            }

            if (title.Equals("Hired") || title.Equals("Reject") || title.Equals("KIV"))
            {
                
                interview.IntervieweeStatus = title;
                if (title.Equals("Hired"))
                {
                    /* db.User.Add(new User
                     {
                         Username = "Gordon",
                         Roles = "HR",
                         Email = "gordon@gmail.com",
                         Password ="12345"
                     });*/
                    db.OnBoard.Add(new OnBoard
                    {
                        CandidateName = interview.IntervieweeName,
                        CandidateId = interview.CandidatesId,
                        CompanyType = "Pending",
                        ReasonOfRejectOffer = "None",
                        CheckListStatus = false,
                        CheckListLeft = 0,
                        OnBoardDate = null,
                        EmailAccount = false,
                        OfferIntervieweeStatus = "Pending",
                        PassportSizePhoto = false,
                        Latest3MonthPaySlip = false,
                        PhotocopyOfCertificates = false,
                        PhotocopyOfNRIC = false,
                        MaxSys_RulesAndRegulation = false,
                        MaxSys_DoorAccessCard = false,
                        MaxSys_CompanyTShirt = false,
                        TMS_DevelopmentStandardsBriefing = false,
                        InternalTrainingAndTrainingMaterial = false,
                        SignNDA = false,
                        SignEmailAndInternetAgreement = false,
                        TMS_EmployeeBadge = false,
                        TMS_Safety = false,
                        TMS_InternetAccess = false,
                        TMS_VPN = false,
                        TMS_EPortal_ELearning = false,
                        TMS_TShirt_Cap_Tupperware = false,
                        TMS_Locker = false,
                        TMS_Laptop = false,
                        MantorAndMantee = false,
                        TMS_ServiceReportAccount = false,
                        SMiT = false,
                        DateTimeCreated = DateTime.Now,
                    });
                }
                db.Entry(interview).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");

            }

            if (title == "FirstInterview" && interview.FirstInterviewerStatus == "Yes")
            {
                return RedirectToAction("UpdateInterviewDetails", new { id = id, title = title, progress = 1, finishInterview = "Yes" });
            }

            else if (title == "SecondInterview" && interview.SecondInterviewerStatus == "Yes")
            {
                return RedirectToAction("UpdateInterviewDetails", new { id = id, title = title, progress = 2, finishInterview = "Yes" });
            }

            else if (title == "SecondInterview" && interview.SecondInterviewerStatus == "No")
            {
                return RedirectToAction("UpdateInterviewDetails", new { id = id, title = title, progress = 2, finishInterview = "No" });
            }
            else if (title == "FirstInterview" && interview.FirstInterviewerStatus == "No")
            {
                return RedirectToAction("UpdateInterviewDetails", new { id = id, title = title, progress = 1, finishInterview = "No" });
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

        // POST: Interview/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection fc, string interviewTime, DateTime interviewDate, string[] selectInterviewer, [Bind(Include = "Id, IntervieweeName, IntervieweeStatus, FirstInterviewStatus, SecondInterviewStatus, IntervieweeResumeLink, CandidateId, DateCreated")] Interview interview)
        {
            bool checkSubmit = false;

            string btnClicked = fc["Message"];

            if (Request.Form["submit"] != null)
            {
                checkSubmit = true;
            }
            else if (Request.Form["process"] != null)
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
                    else if ((interview.FirstInterviewerStatus.Equals("No") || interview.FirstInterviewerStatus.Equals("Yes")) && interview.SecondInterviewerStatus.Equals("TBA"))
                    {
                        progress = 2;
                        interview.SecondInterviewerStatus = "No";
                    }
                    db.InterviewDetail.Add(new InterviewDetail
                    {
                        IntervieweeId = interview.Id,
                        InterviewTime = interviewTime.ToString(),
                        InterviewDate = interviewDate,
                        InterviewProgress = progress,
                        DateCreated = DateTime.Now,
                    });
                    db.SaveChanges();
                    int findInterviewDetailId = (from s in db.InterviewDetail
                                                 where s.IntervieweeId == interview.Id & s.InterviewProgress == progress
                                                 select s.Id).First();

                    for (int i = 0; i < selectInterviewer.Length; i++)
                    {
                        string selectedInterviewer = selectInterviewer[i];
                        User interviewerUser = db.User.Find((from s in db.User
                                                             where s.Username == selectedInterviewer
                                                             select s.Id).First());
                        db.InterviewerComment.Add(new InterviewerComment
                        {
                            InterviewerId = interviewerUser.Id,
                            InterviewDetailId = findInterviewDetailId,
                            InterviewRemarks = "None",
                            InterviewResult = "None",
                            DateCreated = DateTime.Now,
                        });
                    }
                    //switch (selectInterviewer[0])
                    //{
                    //    case "Sky":
                    //        {
                    //            db.InterviewerComment.Add(new InterviewerComment
                    //            {
                    //                InterviewerId = 1,
                    //                InterviewDetailId = findInterviewDetailId,
                    //                InterviewRemarks = "None",
                    //                InterviewResult = "None"
                    //            });
                    //            break;
                    //        }
                    //    case "Danny":
                    //        {
                    //            db.InterviewerComment.Add(new InterviewerComment
                    //            {
                    //                InterviewerId = 2,
                    //                InterviewDetailId = findInterviewDetailId,
                    //                InterviewRemarks = "None",
                    //                InterviewResult = "None"
                    //            });
                    //            break;
                    //        }
                    //    case "Both":
                    //        {
                    //            db.InterviewerComment.Add(new InterviewerComment
                    //            {
                    //                InterviewerId = 1,
                    //                InterviewDetailId = findInterviewDetailId,
                    //                InterviewRemarks = "None",
                    //                InterviewResult = "None"
                    //            });
                    //            db.InterviewerComment.Add(new InterviewerComment
                    //            {
                    //                InterviewerId = 2,
                    //                InterviewDetailId = findInterviewDetailId,
                    //                InterviewRemarks = "None",
                    //                InterviewResult = "None"
                    //            });
                    //            break;
                    //        }
                    //}
                }
                db.Entry(interview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(interview);
        }

        public ActionResult UpdateInterviewDetails(int? id, string title, int? progress, string finishInterview)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviewer.Find(id);

            ViewBag.intervieweeId = interview.CandidatesId;

            /*List<InterviewDetail> data  = db.InterviewDetail.Where(c => c.IntervieweeId == id & c.InterviewProgress == progress).ToList();*/

            ViewBag.intervieweeName = interview.IntervieweeName;
            if (id == null || title == null || progress == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.FinishInterview = finishInterview;
            ViewBag.Message = title;
            ViewBag.id = id;
            /*if (Session["Roles"].Equals("interviewer")) {
                ViewBag.Roles = "I am Interviewer";
            }
            else if(Session["Roles"].Equals("HR"))
            {
                ViewBag.Roles = "I am HR";
            }*/

            int findInterviewDetailId = (from s in db.InterviewDetail
                                         where s.IntervieweeId == id & s.InterviewProgress == progress
                                         select s.Id).First();
            ViewBag.InterviewDetailId = findInterviewDetailId;

            var num = (from s in db.InterviewerComment
                       where s.InterviewDetailId == findInterviewDetailId
                       select s).Count();       // used to calculate how many rows of same interviewee id found in table

            //if (num == 1)
            //{
            //    var interviewerId = (from s in db.InterviewerComment
            //                         where s.InterviewDetailId == findInterviewDetailId
            //                         select s.InterviewerId).First();
            //    ViewBag.interviewer = interviewerId; // used to find which interviewer to display in drop-down list
            //}

            var interviewDate = (from s in db.InterviewDetail
                                 where s.IntervieweeId == id & s.InterviewProgress == progress
                                 select s.InterviewDate).First();

            var interviewTime = (from s in db.InterviewDetail
                                 where s.IntervieweeId == id & s.InterviewProgress == progress
                                 select s.InterviewTime).First();

            //string interviewDetailId = string.Join(",",(from s in db.InterviewDetail
            //                     where s.IntervieweeId == id & s.InterviewProgress == progress
            //                     select s.Id).ToArray());      // used to find interviewDetailId's primary key

            int sessionId = (int)Session["Id"];

            if (Session["Roles"].Equals("interviewer"))
            {
                // check the interview details Id izit exist in database with the session id, if yes will search on the particular data
                if (db.InterviewerComment.Where(u => u.InterviewDetailId == findInterviewDetailId && u.InterviewerId == sessionId).Any())
                {
                    ViewBag.CurrentInterviewCommentSame = "Yes";
                    var currentInterviewerComment = (from s in db.InterviewerComment
                                                     where s.InterviewDetailId == findInterviewDetailId && s.InterviewerId == sessionId
                                                     select s.InterviewerId).First();
                    if (sessionId.Equals(currentInterviewerComment))        // enter here when the session id login is same with the interviewer comment in database
                    {
                        currentInterviewerComment = (from s in db.InterviewerComment
                                                     where s.InterviewDetailId == findInterviewDetailId && s.InterviewerId == sessionId
                                                     select s.InterviewerId).First();

                        if (sessionId.Equals(currentInterviewerComment))
                        {
                            ViewBag.InterviewRemarks = (from s in db.InterviewerComment
                                                        where s.InterviewDetailId == findInterviewDetailId && s.InterviewerId == sessionId
                                                        select s.InterviewRemarks).First();
                            ViewBag.InterviewResult = (from s in db.InterviewerComment
                                                       where s.InterviewDetailId == findInterviewDetailId && s.InterviewerId == sessionId
                                                       select s.InterviewResult).First();
                        }
                    }
                }
                // enter here when the session id login is not exist in interview detail database, will proceed to show other interviewer data
                else
                {
                    ViewBag.CurrentInterviewCommentSame = "No";
                    ViewBag.InterviewRemarks = (from s in db.InterviewerComment
                                                where s.InterviewDetailId == findInterviewDetailId
                                                select s.InterviewRemarks).ToList();
                    ViewBag.InterviewResult = (from s in db.InterviewerComment
                                               where s.InterviewDetailId == findInterviewDetailId
                                               select s.InterviewResult).ToList();
                }
            }
            // enter here when the session roles belong to HR 
            else
            {
                ViewBag.CurrentInterviewCommentSame = "HR";
                ViewBag.InterviewRemarks = (from s in db.InterviewerComment
                                            where s.InterviewDetailId == findInterviewDetailId
                                            select s.InterviewRemarks).ToList();

                ViewBag.InterviewResult = (from s in db.InterviewerComment
                                           where s.InterviewDetailId == findInterviewDetailId
                                           select s.InterviewResult).ToList();
            }

            /* var interviewRemarks = (from s in db.InterviewerComment
                                     where s.InterviewDetailId == findInterviewDetailId
                                     select s.InterviewRemarks).First();

             var interviewResult = (from s in db.InterviewerComment
                                    where s.InterviewDetailId == findInterviewDetailId
                                    select s.InterviewResult).First();*/

            for (int i = 0; i < num; i++)
            {
                var interviewerList = (from s in db.InterviewerComment
                                       where s.InterviewDetailId == findInterviewDetailId
                                       select s.InterviewerId).ToList();
                /*int[] array = interviewerList.ToArray();*/
                ViewBag.InterviewerList = interviewerList;
            }

            List<string> interviewerNameList = new List<string>();
            for (int x = 0; x < num; x++)
            {
                int UserId = ViewBag.InterviewerList[x];
                interviewerNameList.Add((from s in db.User
                                         where s.Id == UserId
                                         select s.Username).First());
                var interviewerName = interviewerNameList.ToList();
                ViewBag.InterviewerName = interviewerName;
            }


            //ViewBag.InterviewDetailId = interviewDetailId;
            ViewBag.Progress = progress.ToString();
            /*ViewBag.InterviewRemarks = interviewRemarks;
            ViewBag.InterviewResult = interviewResult;*/

            ViewBag.InterviewDate = interviewDate;
            ViewBag.InterviewTime = interviewTime;
            ViewBag.num = num;
            return View(interview);
        }

        [HttpPost]
        public ActionResult UpdateInterviewDetails(FormCollection fc, string interviewTime, DateTime? interviewDate, string[] selectInterviewer, string interviewRemarks, string interviewResult, [Bind(Include = "Id, IntervieweeId, IntervieweeUserId, InterviewTime, InterviewDate, IntervieweRemarks, InterviewResult, InterviewProgress")] InterviewDetail interview)
        {
            bool existInterviewer = false;
            int progress = Int32.Parse(fc["Progress"]);
            string interviewerFromDatabase = fc["SelectedInterviewer"];
            int interviewDetailIdDb = Int32.Parse(fc["InterviewDetailId"]);
            int interviewerNum = Int32.Parse(fc["InterviewerNum"]);
            string interviewerIdList = fc["InterviewerList"];
            string finishInterview = fc["FinishInterview"];
            bool editInterviewDetails = false;

            List<string> interviewList = interviewerIdList.Split(',').ToList();
            Interview temp = db.Interviewer.Find(interview.Id);

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
                if (interviewResult == null)
                {
                    interviewResult = "None";
                }
                if (finishInterview.Equals("No"))
                {
                    List<string> interviewNameList = interviewerFromDatabase.Split(',').ToList();
                    InterviewDetail InterviewDetail = db.InterviewDetail.Find(interviewDetailIdDb);
                    InterviewDetail.InterviewDate = (DateTime)interviewDate;
                    InterviewDetail.InterviewTime = interviewTime;
                    db.Entry(InterviewDetail).State = EntityState.Modified;
                    db.SaveChanges();

                    int interviewerCount = interviewNameList.Count;
                    int interviewerFindCount = 0;
                    if (selectInterviewer != null)
                    {
                        if (interviewNameList.Count == selectInterviewer.Length)
                        {
                            foreach (string name in interviewNameList)
                            {
                                foreach (string findName in selectInterviewer)
                                {
                                    if (findName.Equals(name))
                                    {
                                        interviewerFindCount++;
                                    }
                                }
                            }
                        }

                        if (interviewerCount != interviewerFindCount)
                        {
                            editInterviewDetails = true;
                            for (int i = 0; i < interviewerNum; i++)    // find interviewComment id and delete related data
                            {
                                int findId = (from s in db.InterviewerComment
                                              where s.InterviewDetailId == interviewDetailIdDb
                                              select s.Id).First();
                                InterviewerComment interviewComment = db.InterviewerComment.Find(findId);
                                db.InterviewerComment.Remove(interviewComment);
                                db.SaveChanges();
                            }
                            for (int i = 0; i < selectInterviewer.Length; i++)
                            {
                                string selectedInterviewer = selectInterviewer[i];
                                User interviewerUser = db.User.Find((from s in db.User
                                                                     where s.Username == selectedInterviewer
                                                                     select s.Id).First());
                                db.InterviewerComment.Add(new InterviewerComment
                                {
                                    InterviewerId = interviewerUser.Id,
                                    InterviewDetailId = interviewDetailIdDb,
                                    InterviewRemarks = "None",
                                    InterviewResult = "None"
                                });
                            }
                        }
                    }
                }

                for (int i = 0; i < interviewList.Count(); i++)
                {
                    if ((int)Session["Id"] == Int32.Parse(interviewList[i]))
                    {
                        System.Diagnostics.Debug.WriteLine("Interview Exist");
                        existInterviewer = true;
                    }
                }


                if (Session["Roles"].Equals("interviewer") && existInterviewer && editInterviewDetails == false)
                {
                    int sessionId = (int)Session["Id"];
                    int interviewCommentId = (from s in db.InterviewerComment
                                              where s.InterviewDetailId == interviewDetailIdDb && s.InterviewerId == sessionId
                                              select s.Id).First();
                    InterviewerComment interviewComment = db.InterviewerComment.Find(interviewCommentId);

                    if (progress == 1 && interviewRemarks != "None" && interviewResult != "None")
                    {
                        temp.FirstInterviewerStatus = "Yes";
                    }
                    else if (progress == 2 && interviewRemarks != "None" && interviewResult != "None")
                    {
                        temp.SecondInterviewerStatus = "Yes";
                    }
                    interviewComment.InterviewRemarks = interviewRemarks;
                    interviewComment.InterviewResult = interviewResult;
                    db.Entry(interviewComment).State = EntityState.Modified;
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();

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