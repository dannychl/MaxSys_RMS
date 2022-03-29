﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecruitmentManagementSystem__Danny_.DAL;
using RecruitmentManagementSystem__Danny_.Models;

namespace RecruitmentManagementSystem__Danny_.Controllers
{
    public class ReportController : Controller
    {

        private DatabaseContext db = new DatabaseContext();

        [HttpGet]
        public PartialViewResult ShowInterviewReportTable(string searchName, string searchStatus, string startDate, string endDate)
        {
            DateTime startDateConvert = DateTime.Now, endDateConvert = DateTime.Now;
            //InterviewReport myModel = new InterviewReport();

            //myModel.Interviewees = db.Interviewer.ToList();
            //myModel.InterviewDetails = db.InterviewDetail.ToList();
            //myModel.InterviewerComments = db.InterviewerComment.ToList();
            //myModel.Interviewers = db.User.ToList();


            /*InterviewReport interviewReport = new InterviewReport()
            {
                Interviewees = db.Interviewer.ToList(),
                InterviewDetails = db.InterviewDetail.ToList(),
                InterviewerComments = db.InterviewerComment.ToList(),
                Interviewers = db.User.ToList(),
            };*/

            /*interviewReport.Interviewees = db.Interviewer.ToList();
            interviewReport.InterviewDetails = db.InterviewDetail.ToList();
            interviewReport.InterviewerComments = db.InterviewerComment.ToList();
            interviewReport.Interviewers = db.User.ToList();*/

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                startDateConvert = Convert.ToDateTime(startDate);
                endDateConvert = Convert.ToDateTime(endDate);

                if (!string.IsNullOrEmpty(searchName) && !string.IsNullOrEmpty(searchStatus))
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  where (st.InterviewDate >= startDateConvert.Date && st.InterviewDate <= endDateConvert.Date)
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  where s.IntervieweeName == searchName && s.IntervieweeStatus == searchStatus
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
                else if (!string.IsNullOrEmpty(searchName) && string.IsNullOrEmpty(searchStatus))
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  where (st.InterviewDate >= startDateConvert.Date && st.InterviewDate <= endDateConvert.Date)
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  where s.IntervieweeName == searchName
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
                else if (string.IsNullOrEmpty(searchName) && !string.IsNullOrEmpty(searchStatus))
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  where (st.InterviewDate >= startDateConvert.Date && st.InterviewDate <= endDateConvert.Date)
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  where s.IntervieweeStatus == searchStatus
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
                else
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  where (st.InterviewDate >= startDateConvert.Date && st.InterviewDate <= endDateConvert.Date)
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchName) && !string.IsNullOrEmpty(searchStatus))
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  where s.IntervieweeName == searchName && s.IntervieweeStatus == searchStatus
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
                else if(!string.IsNullOrEmpty(searchName) && string.IsNullOrEmpty(searchStatus))
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  where s.IntervieweeName == searchName
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
                else if (string.IsNullOrEmpty(searchName) && !string.IsNullOrEmpty(searchStatus))
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  where s.IntervieweeStatus == searchStatus
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
                else
                {
                    var result = (from s in db.Interviewer.ToList() // outer sequence
                                  join st in db.InterviewDetail.ToList()
                                      on s.Id equals st.IntervieweeId
                                  /*where (st.InterviewDate >= startDateConvert.Date && st.InterviewDate <= endDateConvert.Date)*/
                                  join stt in db.InterviewerComment.ToList()
                                      on st.Id equals stt.InterviewDetailId
                                  join sttt in db.User.ToList()
                                      on stt.InterviewerId equals sttt.Id
                                  join stttt in db.Candidate.ToList()
                                      on s.CandidatesId equals stttt.Id
                                  select new InterviewReport()
                                  { // result selector 
                                      Interviewee = s,
                                      InterviewDetail = st,
                                      InterviewerComment = stt,
                                      Interviewer = sttt,
                                      Candidate = stttt
                                  }).ToList();

                    return PartialView("InterviewReportTable_PartialView", result);
                }
            }
        }

        public PartialViewResult ShowRecruitmentReportTable(string searchMethodUsed, string searchPosition)
        {

            if (!string.IsNullOrEmpty(searchMethodUsed) && !string.IsNullOrEmpty(searchPosition))
            {
                var result = (from s in db.Candidate
                              where s.MethodUsed == searchMethodUsed && s.Position == searchPosition
                              select s).ToList();
                return PartialView("RecruitmentReportTable_PartialView", result);
            }
            else if ((string.IsNullOrEmpty(searchMethodUsed) && !string.IsNullOrEmpty(searchPosition)))
            {
                var result = (from s in db.Candidate
                              where s.Position == searchPosition
                              select s).ToList();
                return PartialView("RecruitmentReportTable_PartialView", result);
            }
            else if (!string.IsNullOrEmpty(searchMethodUsed) && string.IsNullOrEmpty(searchPosition))
            {
                var result = (from s in db.Candidate
                              where s.MethodUsed == searchMethodUsed
                              select s).ToList();
                return PartialView("RecruitmentReportTable_PartialView", result);
            }
            else
            {
                var result = (from s in db.Candidate
                              select s).ToList();
                return PartialView("RecruitmentReportTable_PartialView", result);
            }
        }

            // GET: Report
            public ActionResult Index()
        {
            return View();
        }

        public ActionResult RecruitmentReport()
        {
            Candidate candidate = new Candidate();
            var query = (from s in db.Candidate
                         select s).ToList();


            candidate.MethodUsedList = new SelectList(query.Select(x => x.MethodUsed).Distinct(), "MethodUsed");
            candidate.PositionList = new SelectList(query.Select(x => x.Position).Distinct(), "Position");

            return View(candidate);
        }
        [HttpPost]
        public ActionResult RecruitmentReport(string ddlMethodUsed, string ddlPosition)
        {



            return RedirectToAction("RecruitmentReport");
        }
        public ActionResult InterviewReport()
        {
            Interview interviewee = new Interview();
            var query = (from s in db.Interviewer
                         select s).ToList();


            interviewee.IntervieweeNameList = new SelectList(query.Select(x => x.IntervieweeName).Distinct(), "IntervieweeName");
            interviewee.IntervieweeStatusList = new SelectList(query.Select(x => x.IntervieweeStatus).Distinct(), "IntervieweeStatus");
            return View(interviewee);
        }
        public ActionResult OnBoardingReport()
        {

            return View();
        }
    }
}