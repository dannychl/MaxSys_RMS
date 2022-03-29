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
    public class OnBoardingController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        Interview Interviewer = new Interview();
        InterviewDetail InterviewDetail = new InterviewDetail();
        User Users = new User();

        [HttpGet]
        public PartialViewResult SearchUsers(string searchText)
        {
            var model = (from s in db.OnBoard
                        where s.CandidateName.ToLower().Contains(searchText)
                        select s).ToList();
            return PartialView("SearchOnBoardCandidate_View", model);
        }
        // GET: OnBoarding
        public ActionResult Index()
        {
            var model = (from s in db.OnBoard
                         select s).ToList();

            return View(model);
        }

        // GET: OnBoarding/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OnBoarding/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnBoarding/Create
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

        public ActionResult DisplayDepartmentWithCheckBox(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OnBoard onBoard = db.OnBoard.Find(id);

            if (onBoard.CompanyType != null)
            {
                ViewBag.companyType = onBoard.CompanyType;
            }
            return View(onBoard);
        }

        [HttpPost]
        public ActionResult DisplayDepartmentWithCheckBox(string btnCompanyType, DateTime interviewDate, DateTime? interviewDate2, DateTime? interviewDate3, [Bind(Include ="Id, CandidateName,OfferIntervieweeStatus,PassportSizePhoto,Latest3MonthPaySlip,PhotocopyOfCertificates,PhotocopyOfNRIC,MaxSys_RulesAndRegulation,MaxSys_DoorAccessCard,EmailAccount,MaxSys_CompanyTShirt,TMS_DevelopmentStandardsBriefing,InternalTrainingAndTrainingMaterial,SignNDA,SignEmailAndInternetAgreement,TMS_EmployeeBadge,TMS_Safety,TMS_InternetAccess,TMS_VPN,TMS_EPortal_ELearning,TMS_TShirt_Cap_Tupperware,TMS_Locker,TMS_Laptop,MantorAndMantee,TMS_ServiceReportAccount,SMiT,DateTimeCreated,CandidateId,CheckListStatus")] OnBoard onBoard)
        {
            List<bool> lstTest = new List<bool>();
            var modify = db.OnBoard.Find(onBoard.Id);
            if (btnCompanyType != null)
            {
                if (btnCompanyType == "TMS")
                {
                    modify.CompanyType = "TMS";
                }
                else if (btnCompanyType == "Max")
                {
                    modify.CompanyType = "Max";
                }
                modify.OnBoardDate = interviewDate;
                db.Entry(modify).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DisplayDepartmentWithCheckBox", new { id = onBoard.Id });
            }
            else
            {
                
                if (modify.CompanyType == "TMS")
                { 
                    modify.SignNDA = onBoard.SignNDA;
                    modify.SignEmailAndInternetAgreement = onBoard.SignEmailAndInternetAgreement;
                    modify.TMS_EmployeeBadge = onBoard.TMS_EmployeeBadge;
                    modify.TMS_Safety = onBoard.TMS_Safety;
                    modify.TMS_InternetAccess = onBoard.TMS_InternetAccess;
                    modify.TMS_VPN = onBoard.TMS_VPN;
                    modify.TMS_EPortal_ELearning = onBoard.TMS_EPortal_ELearning;
                    modify.TMS_TShirt_Cap_Tupperware = onBoard.TMS_TShirt_Cap_Tupperware;
                    modify.TMS_Locker = onBoard.TMS_Locker;
                    modify.TMS_Laptop = onBoard.TMS_Laptop;
                    modify.TMS_DevelopmentStandardsBriefing = onBoard.TMS_DevelopmentStandardsBriefing;
                    modify.InternalTrainingAndTrainingMaterial = onBoard.InternalTrainingAndTrainingMaterial;
                    modify.MantorAndMantee = onBoard.MantorAndMantee;
                    modify.TMS_ServiceReportAccount = onBoard.TMS_ServiceReportAccount;
                    modify.SMiT = onBoard.SMiT;

                    if (modify.SignNDA && modify.SignEmailAndInternetAgreement && modify.TMS_EmployeeBadge && modify.TMS_Safety
                       && modify.TMS_InternetAccess && modify.TMS_VPN && modify.TMS_EPortal_ELearning && modify.TMS_TShirt_Cap_Tupperware
                       && modify.TMS_Locker && modify.TMS_Laptop && modify.TMS_DevelopmentStandardsBriefing && modify.InternalTrainingAndTrainingMaterial
                       && modify.MantorAndMantee && modify.TMS_ServiceReportAccount && modify.SMiT)
                    {
                        modify.CheckListStatus = true;
                    }
                    else
                    {
                        modify.CheckListStatus = false;
                    }

                    lstTest.Add(onBoard.SignNDA);
                    lstTest.Add(onBoard.SignEmailAndInternetAgreement);
                    lstTest.Add(onBoard.TMS_EmployeeBadge);
                    lstTest.Add(onBoard.TMS_Safety);
                    lstTest.Add(onBoard.TMS_InternetAccess);
                    lstTest.Add(onBoard.TMS_VPN);
                    lstTest.Add(onBoard.TMS_EPortal_ELearning);
                    lstTest.Add(onBoard.TMS_TShirt_Cap_Tupperware);
                    lstTest.Add(onBoard.TMS_Locker);
                    lstTest.Add(onBoard.TMS_Laptop);
                    lstTest.Add(onBoard.TMS_DevelopmentStandardsBriefing);
                    lstTest.Add(onBoard.InternalTrainingAndTrainingMaterial);
                    lstTest.Add(onBoard.MantorAndMantee);
                    lstTest.Add(onBoard.TMS_ServiceReportAccount);
                    lstTest.Add(onBoard.SMiT);

                    int checkFalse = 0;
                    foreach (var c in lstTest)
                    {
                        if (!c)
                        {
                            checkFalse++;
                        }

                        if (c == lstTest.Last())
                        {
                            modify.CheckListLeft = checkFalse;
                        }
                    }
                    modify.OnBoardDate = interviewDate2;
                }
                else if (modify.CompanyType == "Max")
                {
                    modify.PassportSizePhoto = onBoard.PassportSizePhoto;
                    modify.Latest3MonthPaySlip = onBoard.Latest3MonthPaySlip;
                    modify.PhotocopyOfCertificates = onBoard.PhotocopyOfCertificates;
                    modify.PhotocopyOfNRIC = onBoard.PhotocopyOfNRIC;
                    modify.MaxSys_RulesAndRegulation = onBoard.MaxSys_RulesAndRegulation;
                    modify.MaxSys_DoorAccessCard = onBoard.MaxSys_DoorAccessCard;
                    modify.EmailAccount = onBoard.EmailAccount;
                    modify.MaxSys_CompanyTShirt = onBoard.MaxSys_CompanyTShirt;
                    modify.TMS_DevelopmentStandardsBriefing = onBoard.TMS_DevelopmentStandardsBriefing;
                    modify.InternalTrainingAndTrainingMaterial = onBoard.InternalTrainingAndTrainingMaterial;
                    if (modify.PassportSizePhoto && modify.Latest3MonthPaySlip && modify.PhotocopyOfCertificates && modify.PhotocopyOfNRIC
                        && modify.MaxSys_RulesAndRegulation && modify.MaxSys_DoorAccessCard && modify.EmailAccount && modify.MaxSys_CompanyTShirt
                        && modify.TMS_DevelopmentStandardsBriefing && modify.InternalTrainingAndTrainingMaterial)
                    {
                        modify.CheckListStatus = true;
                    }
                    else
                    {
                        modify.CheckListStatus = false;
                    }

                    lstTest.Add(onBoard.PassportSizePhoto);
                    lstTest.Add(onBoard.Latest3MonthPaySlip);
                    lstTest.Add(onBoard.PhotocopyOfCertificates);
                    lstTest.Add(onBoard.PhotocopyOfNRIC);
                    lstTest.Add(onBoard.MaxSys_RulesAndRegulation);
                    lstTest.Add(onBoard.MaxSys_DoorAccessCard);
                    lstTest.Add(onBoard.EmailAccount);
                    lstTest.Add(onBoard.MaxSys_CompanyTShirt);
                    lstTest.Add(onBoard.TMS_DevelopmentStandardsBriefing);
                    lstTest.Add(onBoard.InternalTrainingAndTrainingMaterial);

                    int checkFalse = 0;
                    foreach (var c in lstTest)
                    {
                        if (!c)
                        {
                            checkFalse++;
                        }

                        if (c == lstTest.Last())
                        {
                            modify.CheckListLeft = checkFalse;
                        }
                    }
                    modify.OnBoardDate = interviewDate3;
                }
            }

            db.Entry(modify).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: OnBoarding/Edit/5
        public ActionResult Edit(int? id, string title)
        {
            OnBoard onBoard = db.OnBoard.Find(id);
            if (title.Equals("Accept") || title.Equals("Reject"))
            {
                /*if (title.Equals("Reject"))
                {
                    ViewBag.ConfirmReject = title;
                }*/
                ViewBag.searchStatus = title;
                onBoard.OfferIntervieweeStatus = title;
                db.Entry(onBoard).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: OnBoarding/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id)
        {


            return View();
        }

        // GET: OnBoarding/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OnBoarding/Delete/5
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
