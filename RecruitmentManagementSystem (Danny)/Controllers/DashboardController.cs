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
    public class DashboardController : Controller
    {

        private DatabaseContext db = new DatabaseContext();
        Candidate Candidate = new Candidate();
        Interview Interview = new Interview();

        // GET: Dashboard
        public ActionResult Index()
        {
            // 1. Count how many interviewer waiting to interview (how many left)
            // 2. Total hired/ KIV
            // 3. 

           
            var interviewStatusList = db.Interviewer.ToList();
            var list = db.Candidate.Where(x => x.ProgrammingTest == 0 && x.SQLTest == 0).ToList();
            var interviewee = (from s in db.Interviewer // outer sequence
                              join st in db.Candidate
                                  on s.CandidatesId equals st.Id
                              where s.FirstInterviewerStatus == "No" || s.SecondInterviewerStatus == "No"
                              select new
                              { // result selector 
                                  Candidate = st,
                                  Interviewer = s
                              }).ToList();
            List<int> testCount = new List<int>();
            List<int> notInterviewCount = new List<int>();
            List<int> interviewStatusCount = new List<int>();
            var position = list.Select(x => x.Position).Distinct();    
            var interviewStatus = interviewStatusList.Select(x => x.IntervieweeStatus).Distinct();

            foreach (var item in position)
            {
                testCount.Add(list.Count(x => x.Position == item));
                notInterviewCount.Add(interviewee.Count(x => x.Candidate.Position == item)/*list.Count(x => x.Position == items.Candidate.Position)*/);
            }

            foreach (var interviewStat in interviewStatus)
            {
                interviewStatusCount.Add(interviewStatusList.Count(x => x.IntervieweeStatus == interviewStat));
            }

            var count = testCount;
            ViewBag.Position = position;                                //All Candidates Position
            ViewBag.NotInterviewCount = notInterviewCount.ToList();     //All Haven't Interviewed Candidates
            ViewBag.TestCount = testCount.ToList();                     //All Haven't Test Candidates

            ViewBag.InterviewStatus = interviewStatus;                  //All InterviewStatus e.g KIV, Pending, Hired, Reject
            ViewBag.InterviewStatusCount = interviewStatusCount.ToList(); //Total of InterviewStatus

            return View();
        }

        // GET: Dashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
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

        // GET: Dashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dashboard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
