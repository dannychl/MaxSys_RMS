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
using System.IO;
using ClosedXML.Excel;

namespace RecruitmentManagementSystem__Danny_.Controllers
{
    public class ShowAllCandidatesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        Candidate Candidate = new Candidate();
        Interview Interview = new Interview();

        // GET: ShowAllCandidates
    /*    public ActionResult Index(int? page)
        {
            
            int pageSize = 5;
            int pageIndex = 1;

            IPagedList<Candidate> candidates = null;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<Candidate> objCandidateList = db.Candidate.ToList();
            candidates = objCandidateList.ToPagedList(pageIndex, pageSize);

         
       

            *//*var model = db.Candidate.OrderBy(s => s.Id).Skip(intSkip).Take(intPageSize).ToList().ToPagedList(page ?? intPage, maxRows);*//*
         

            return View(candidates);
        }*/

        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult SearchUsers(string searchText/*, int? page*/)
        {

            var model = from s in db.Candidate
                        select s;

            /*int pageSize = 10;
            int pageIndex = 1;

            IPagedList<Candidate> candidates = null;*/

            /* pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
             IPagedList<Candidate> candidates = null;
             List<Candidate> objCandidateList = model.Where(a => a.Name.ToLower().Contains(searchText) || a.Position.ToLower().Contains(searchText)).ToList();
             candidates = objCandidateList.ToPagedList(pageIndex, pageSize);*/

            /*if (searchText != null && searchText != "")
            {
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                List<Candidate> objCandidateList = model.Where(a => a.Name.ToLower().Contains(searchText) || a.Position.ToLower().Contains(searchText)).ToList();
                candidates = objCandidateList.ToPagedList(pageIndex, pageSize);
            }
            else
            {
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                List<Candidate> objCandidateList = (from s in db.Candidate select s).ToList();
                candidates = objCandidateList.ToPagedList(pageIndex, pageSize);
            }*/

            var result = model.Where(a => a.Name.ToLower().Contains(searchText));

            return PartialView("SearchAllCandidate_View", result);
        }

        [HttpPost]
        public FileResult Export()
        {
            DatabaseContext db = new DatabaseContext();
            DataTable dt = new DataTable("Candidate (Personal Details)");
            DataTable dt2 = new DataTable("Candidate (Company Details)");
            DataTable dt3 = new DataTable("Candidate (Interview Details)");
            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("Id"),
                                            new DataColumn("Name"),
                                            new DataColumn("Age"),
                                            new DataColumn("DateOfBirth"),
                                            new DataColumn("Gender"),
                                            new DataColumn("PhoneNum"),
                                            new DataColumn("CurrentSalary"),
                                            new DataColumn("ExpectedSalary"),
                                            new DataColumn("MethodUsed (For Recruit)") });


            dt2.Columns.AddRange(new DataColumn[12] { new DataColumn("Id"),
                                            new DataColumn("Name"),
                                            new DataColumn("Position"),
                                            new DataColumn("WorkingExperience (Year)"),
                                            new DataColumn("WorkingExperienceRemarks"),
                                            new DataColumn("Status (Accept Or Decline PhoneCall)"),
                                            new DataColumn("ProgrammingTest"),
                                            new DataColumn("SQLTest"),
                                            new DataColumn("ResignPeriod (Month)"),
                                            new DataColumn("ResumeLink"),
                                            new DataColumn("TestAnsLink"),
                                            new DataColumn("TestRemarks")});
            
            
            dt3.Columns.AddRange(new DataColumn[8] { new DataColumn("CandidatesId"),
                                            new DataColumn("IntervieweeName"),
                                            new DataColumn("InterviewProgress"),
                                            new DataColumn("InterviewDate"),
                                            new DataColumn("InterviewTime"),
                                            new DataColumn("InterviewRemarks"),
                                            new DataColumn("InterviewResult"),
                                            new DataColumn("Interviewer Name")});


            var candidates = from s in db.Candidate
                             select s;
            var interviewee = from s in db.InterviewDetail // outer sequence
                              join st in db.Interviewer
                                  on s.IntervieweeId equals st.Id
                              join ss in db.Candidate
                                 on st.CandidatesId equals ss.Id
                              join st2 in db.InterviewerComment
                                  on s.Id equals st2.InterviewDetailId
                              join st3 in db.User//inner sequence 
                                  on st2.InterviewerId equals st3.Id // key selector 
                              select new
                              { // result selector 
                                  InterviewerDetails = s,
                                  Candidate = ss,
                                  Interviewer = st,
                                  InterviewComment = st2,
                                  InterviewerUser = st3
                              };

            foreach (var candidate in candidates)
            {
                dt.Rows.Add(candidate.Id, candidate.Name, candidate.Age, candidate.DateOfBirth, candidate.Gender,
                    candidate.PhoneNum, candidate.CurrentSalary, candidate.ExpectedSalary, candidate.MethodUsed);
                dt2.Rows.Add(candidate.Id, candidate.Name, candidate.Position, candidate.WorkingExperience, candidate.WorkingExperienceRemarks,
                    candidate.Status, candidate.ProgrammingTest, candidate.SQLTest, candidate.ResignPeriod, candidate.ResumeLink,
                    candidate.TestAnsLink, candidate.TestRemarks);
            }

            

            foreach (var interview in interviewee)
            {
                string tempTime = interview.InterviewerDetails.InterviewTime;
                string formattedTime = "";
                int formattedHour = Int32.Parse(tempTime.Substring(0, 2));
                if (formattedHour > 12)
                {
                    formattedHour -= 12;
                    formattedTime = Convert.ToString(formattedHour) + tempTime.Substring(2, 3) + " am";
                }
                else
                {
                    formattedTime = tempTime + " pm";
                }

                dt3.Rows.Add(interview.Interviewer.CandidatesId, interview.Candidate.Name, interview.InterviewerDetails.InterviewProgress, @Convert.ToDateTime(interview.InterviewerDetails.InterviewDate).ToString("dd-MMM-yyyy"), formattedTime,
                    interview.InterviewComment.InterviewRemarks, interview.InterviewComment.InterviewResult, interview.InterviewerUser.Username);
            }
            

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Worksheets.Add(dt2);
                wb.Worksheets.Add(dt3);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SummaryOfCandidateDetails.xlsx");
                }
            }
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
        public ActionResult Edit(FormCollection fc, [Bind(Include = "Id,Name,Age,Position,ExpectedSalary, DateOfBirth, Gender, WorkingExperience, WorkingExperienceRemarks, ResignPeriod, ProgrammingTest, SQLTest, TestRemarks, Status, CurrentSalary, MethodUsed, PhoneNum, ResumeLink, TestAnsLink, DateCreated")] Candidate home)
        {
            if (ModelState.IsValid)
            {
                home.DateCreated = DateTime.Parse(fc["DateCreated"]);
                int currentAge = home.DateOfBirth.Year;
                int curAge = DateTime.Now.Year;

                int exactAge = curAge - currentAge;

                home.Age = exactAge;
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
