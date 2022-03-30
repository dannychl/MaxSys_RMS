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
    public class ReportController : Controller
    {

        private DatabaseContext db = new DatabaseContext();


        public ActionResult OnBoardingReport()
        {
            OnBoard onboard = new OnBoard();
            var query = (from s in db.OnBoard
                         select s).ToList();

            onboard.CandidateNameList = new SelectList(query.Select(x => x.CandidateName).Distinct(), "CandidateName");
            onboard.OfferCandidateStatusList = new SelectList(query.Select(x => x.OfferIntervieweeStatus).Distinct(), "OfferCandidateStatus");
            return View(onboard);
        }

        [HttpPost]
        public ActionResult OnBoardingReport(string ddlCandidateName, string ddlOfferCandidateStatus, string txtStartDate, string txtEndDate)
        {
            DataTable dt = new DataTable("On-Boarding Report");

            dt.Columns.AddRange(new DataColumn[28] { new DataColumn("CandidatesId"),
                                            new DataColumn("CandidateName"),
                                            new DataColumn("OfferIntervieweeStatus"),
                                            new DataColumn("PassportSizePhoto"),
                                            new DataColumn("Latest3MonthPaySlip"),
                                            new DataColumn("PhotocopyOfCertificates"),
                                            new DataColumn("PhotocopyOfNRIC"),
                                            new DataColumn("MaxSys_RulesAndRegulation"),
                                            new DataColumn("MaxSys_DoorAccessCard"),
                                            new DataColumn("EmailAccount"),
                                            new DataColumn("MaxSys_CompanyTShirt"),
                                            new DataColumn("TMS_DevelopmentStandardsBriefing"),
                                            new DataColumn("InternalTrainingAndTrainingMaterial"),
                                            new DataColumn("SignNDA"),
                                            new DataColumn("SignEmailAndInternetAgreement"),
                                            new DataColumn("TMS_EmployeeBadge"),
                                            new DataColumn("TMS_Safety"),
                                            new DataColumn("TMS_InternetAccess"),
                                            new DataColumn("TMS_VPN"),
                                            new DataColumn("TMS_EPortal_ELearning "),
                                            new DataColumn("TMS_TShirt_Cap_Tupperware"),
                                            new DataColumn("TMS_Locker"),
                                            new DataColumn("TMS_Laptop"),
                                            new DataColumn("MantorAndMantee"),
                                            new DataColumn("TMS_ServiceReportAccount"),
                                            new DataColumn("SMiT"),
                                            new DataColumn("DateTimeCreated"),
                                            new DataColumn("CheckListStatus (Completed?)")});

            var result = OnBoardReportChecking(ddlCandidateName, ddlOfferCandidateStatus, txtStartDate, txtEndDate);

            foreach (var candidate in result)
            {
                dt.Rows.Add(candidate.CandidateId, candidate.CandidateName, candidate.OfferIntervieweeStatus, candidate.PassportSizePhoto, candidate.Latest3MonthPaySlip, candidate.PhotocopyOfCertificates, candidate.PhotocopyOfNRIC,
                    candidate.MaxSys_RulesAndRegulation, candidate.MaxSys_DoorAccessCard, candidate.EmailAccount, candidate.MaxSys_CompanyTShirt, candidate.TMS_DevelopmentStandardsBriefing,
                    candidate.InternalTrainingAndTrainingMaterial, candidate.SignNDA, candidate.SignEmailAndInternetAgreement, candidate.TMS_EmployeeBadge, candidate.TMS_Safety, candidate.TMS_InternetAccess,
                    candidate.TMS_VPN, candidate.TMS_EPortal_ELearning, candidate.TMS_TShirt_Cap_Tupperware, candidate.TMS_Locker, candidate.TMS_Laptop, candidate.MantorAndMantee, candidate.TMS_ServiceReportAccount, candidate.SMiT,
                    candidate.DateTimeCreated, candidate.CheckListStatus);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SummaryOf_OnBoardReport.xlsx");
                }
            }
        }
        public IQueryable<OnBoard> OnBoardReportChecking(string ddlCandidateName, string ddlOfferCandidateStatus, string onBoardDateFrom, string onBoardDateTo)
        {
            DateTime startDateConvert = DateTime.Now, endDateConvert = DateTime.Now;
            if (!string.IsNullOrEmpty(onBoardDateFrom) && !string.IsNullOrEmpty(onBoardDateTo))
            {
                startDateConvert = Convert.ToDateTime(onBoardDateFrom);
                endDateConvert = Convert.ToDateTime(onBoardDateTo);

                if (!string.IsNullOrEmpty(ddlCandidateName) && !string.IsNullOrEmpty(ddlOfferCandidateStatus))
                {
                    var result = (from s in db.OnBoard
                                  where s.CandidateName == ddlCandidateName && s.OfferIntervieweeStatus == ddlOfferCandidateStatus && (s.OnBoardDate >= startDateConvert.Date && s.OnBoardDate <= endDateConvert.Date)
                                  select s);

                    return result;
                }
                else if (!string.IsNullOrEmpty(ddlCandidateName) && string.IsNullOrEmpty(ddlOfferCandidateStatus))
                {
                    var result = (from s in db.OnBoard
                                  where s.CandidateName == ddlCandidateName && (s.OnBoardDate >= startDateConvert.Date && s.OnBoardDate <= endDateConvert.Date)
                                  select s);

                    return result;
                }
                else if (string.IsNullOrEmpty(ddlCandidateName) && !string.IsNullOrEmpty(ddlOfferCandidateStatus))
                {
                    var result = (from s in db.OnBoard
                                  where s.OfferIntervieweeStatus == ddlOfferCandidateStatus && (s.OnBoardDate >= startDateConvert.Date && s.OnBoardDate <= endDateConvert.Date)
                                  select s);

                    return result;
                }
                else
                {
                    var result = (from s in db.OnBoard
                                  where (s.OnBoardDate >= startDateConvert.Date && s.OnBoardDate <= endDateConvert.Date)
                                  select s);
                    return result;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlCandidateName) && !string.IsNullOrEmpty(ddlOfferCandidateStatus))
                {
                    var result = (from s in db.OnBoard
                                  where s.CandidateName == ddlCandidateName && s.OfferIntervieweeStatus == ddlOfferCandidateStatus
                                  select s);

                    return result;
                }
                else if (!string.IsNullOrEmpty(ddlCandidateName) && string.IsNullOrEmpty(ddlOfferCandidateStatus))
                {
                    var result = (from s in db.OnBoard
                                  where s.CandidateName == ddlCandidateName
                                  select s);

                    return result;
                }
                else if (string.IsNullOrEmpty(ddlCandidateName) && !string.IsNullOrEmpty(ddlOfferCandidateStatus))
                {
                    var result = (from s in db.OnBoard
                                  where s.OfferIntervieweeStatus == ddlOfferCandidateStatus
                                  select s);

                    return result;
                }
                else
                {
                    var result = (from s in db.OnBoard
                                  select s);
                    return result;
                }

            }
        }

        public PartialViewResult ShowOnBoardReportTable(string ddlCandidateName, string ddlOfferCandidateStatus, string onBoardDateFrom, string onBoardDateTo)
        {

            var result = OnBoardReportChecking(ddlCandidateName, ddlOfferCandidateStatus, onBoardDateFrom, onBoardDateTo);
            return PartialView("OnboardReportTable_PartialView", result.ToList());
        }

        public PartialViewResult ShowRecruitmentReportTable(string searchMethodUsed, string searchPosition, string dateFrom, string dateTo)
        {

            var result = RecruitmentReportChecking(searchMethodUsed, searchPosition, dateFrom, dateTo);

            return PartialView("RecruitmentReportTable_PartialView", result.ToList());
        }

        public IQueryable<Candidate> RecruitmentReportChecking(string searchMethodUsed, string searchPosition, string dateFrom, string dateTo)
        {
            DateTime startDateConvert = DateTime.Now, endDateConvert = DateTime.Now;
            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                startDateConvert = Convert.ToDateTime(dateFrom);
                endDateConvert = Convert.ToDateTime(dateTo);

                if (!string.IsNullOrEmpty(searchMethodUsed) && !string.IsNullOrEmpty(searchPosition))
                {
                    var result = (from s in db.Candidate
                                  where s.MethodUsed == searchMethodUsed && s.Position == searchPosition && (s.DateCreated >= startDateConvert.Date && s.DateCreated <= endDateConvert.Date)
                                  select s);
                    return result;
                }
                else if ((string.IsNullOrEmpty(searchMethodUsed) && !string.IsNullOrEmpty(searchPosition)))
                {
                    var result = (from s in db.Candidate
                                  where s.Position == searchPosition && (s.DateCreated >= startDateConvert.Date && s.DateCreated <= endDateConvert.Date)
                                  select s);
                    return result;
                }
                else if (!string.IsNullOrEmpty(searchMethodUsed) && string.IsNullOrEmpty(searchPosition))
                {
                    var result = (from s in db.Candidate
                                  where s.MethodUsed == searchMethodUsed && (s.DateCreated >= startDateConvert.Date && s.DateCreated <= endDateConvert.Date)
                                  select s);
                    return result;
                }
                else
                {
                    var result = (from s in db.Candidate
                                  where (s.DateCreated >= startDateConvert.Date && s.DateCreated <= endDateConvert.Date)
                                  select s);
                    return result;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchMethodUsed) && !string.IsNullOrEmpty(searchPosition))
                {
                    var result = (from s in db.Candidate
                                  where s.MethodUsed == searchMethodUsed && s.Position == searchPosition
                                  select s);

                    return result;
                }
                else if (!string.IsNullOrEmpty(searchMethodUsed) && string.IsNullOrEmpty(searchPosition))
                {
                    var result = (from s in db.Candidate
                                  where s.MethodUsed == searchMethodUsed
                                  select s);

                    return result;
                }
                else if (string.IsNullOrEmpty(searchMethodUsed) && !string.IsNullOrEmpty(searchPosition))
                {
                    var result = (from s in db.Candidate
                                  where s.Position == searchPosition
                                  select s);

                    return result;
                }
                else
                {
                    var result = (from s in db.Candidate
                                  select s);

                    return result;
                }
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
        public ActionResult RecruitmentReport(string ddlMethodUsed, string ddlPosition, string dateFrom, string dateTo)
        {
            DataTable dt = new DataTable("Recruitment Report");
            dt.Columns.AddRange(new DataColumn[20] { new DataColumn("Id"),
                                             new DataColumn("Name"),
                                             new DataColumn("Age"),
                                             new DataColumn("Position"),
                                             new DataColumn("DateOfBirth"),
                                             new DataColumn("CurrentSalary"),
                                             new DataColumn("ExpectedSalary"),
                                             new DataColumn("Gender"),
                                             new DataColumn("PhoneNum"),
                                             new DataColumn("ResignPeriod"),
                                             new DataColumn("Working Experience"),
                                             new DataColumn("Working Experience Remarks"),
                                             new DataColumn("Programming Test"),
                                             new DataColumn("SQL Test"),
                                             new DataColumn("Test Remarks"),
                                             new DataColumn("Accept for Interview Status"),
                                             new DataColumn("MethodUsed (For Recruit)"),
                                             new DataColumn("Resume Link"),
                                             new DataColumn("Test Answer Link"),
                                             new DataColumn("Date Created")});

            var result = RecruitmentReportChecking(ddlMethodUsed, ddlPosition, dateFrom, dateTo);

            if (result.ToList().Count() == 0|| result.ToList() == null)
            {
                ViewBag.ErrorMessage = "No Data Found!";
                return View();
            }
            foreach (var candidate in result)
            {
                dt.Rows.Add(candidate.Id, candidate.Name, candidate.Age, candidate.Position, candidate.DateOfBirth, candidate.CurrentSalary, candidate.ExpectedSalary,
                    candidate.Gender, candidate.PhoneNum, candidate.ResignPeriod, candidate.WorkingExperience, candidate.WorkingExperienceRemarks,
                    candidate.ProgrammingTest, candidate.SQLTest, candidate.TestRemarks, candidate.Status, candidate.MethodUsed, candidate.ResumeLink,
                    candidate.TestAnsLink, candidate.DateCreated);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SummaryOf_RecruitmentReport.xlsx");
                }
            }

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
        [HttpPost]
        public ActionResult InterviewReport(string ddlIntervieweeName, string ddlIntervieweeStatus, string txtStartDate, string txtEndDate)
        {
            DatabaseContext db = new DatabaseContext();
            DataTable dt = new DataTable("Recruitment Report");

            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("CandidatesId"),
                                            new DataColumn("IntervieweeName"),
                                            new DataColumn("InterviewProgress"),
                                            new DataColumn("InterviewDate"),
                                            new DataColumn("InterviewTime"),
                                            new DataColumn("InterviewRemarks"),
                                            new DataColumn("InterviewResult"),
                                            new DataColumn("Interviewer Name")});

            var result = InterviewReportChecking(ddlIntervieweeName, ddlIntervieweeStatus, txtStartDate, txtEndDate);

            foreach (var interview in result)
            {
                string tempTime = interview.InterviewDetail.InterviewTime;
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
                dt.Rows.Add(interview.Interviewee.CandidatesId, interview.Candidate.Name, interview.InterviewDetail.InterviewProgress, @Convert.ToDateTime(interview.InterviewDetail.InterviewDate).ToString("dd-MMM-yyyy"), formattedTime,
                   interview.InterviewerComment.InterviewRemarks, interview.InterviewerComment.InterviewResult, interview.Interviewer.Username);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SummaryOf_InterviewReport.xlsx");
                }
            }
        }

        public IEnumerable<InterviewReport> InterviewReportChecking(string searchName, string searchStatus, string startDate, string endDate)
        {
            DateTime startDateConvert = DateTime.Now, endDateConvert = DateTime.Now;

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
                                  });

                    return result;
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
                                  });

                    return result;
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
                                  });

                    return result;
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
                                  });

                    return result;
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
                                  });

                    return result;
                }
                else if (!string.IsNullOrEmpty(searchName) && string.IsNullOrEmpty(searchStatus))
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
                                  });

                    return result;
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
                                  });

                    return result;
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
                                  });

                    return result;
                }

            }
        }

        [HttpGet]
        public PartialViewResult ShowInterviewReportTable(string searchName, string searchStatus, string startDate, string endDate)
        {

            var result = InterviewReportChecking(searchName, searchStatus, startDate, endDate);

            return PartialView("InterviewReportTable_PartialView", result.ToList());
        }

        public FileResult exportFile(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SummaryOfCandidateDetails.xlsx");
                }
            }
        }
    }
}