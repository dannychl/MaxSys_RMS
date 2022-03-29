using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RecruitmentManagementSystem__Danny_.Models
{
    [Table("dbo.OnBoarding")]
    public class OnBoard
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Candidate Name")]
        public string CandidateName { get; set; }
        public string CompanyType { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OnBoardDate { get; set; }
        public int CandidateId { get; set; }
        public int CheckListLeft { get; set; }
        public bool CheckListStatus { get; set; }

        [DisplayName("Offer Interviewee Status")]
        public string OfferIntervieweeStatus { get; set; }

        public bool PassportSizePhoto { get; set; }

        public bool Latest3MonthPaySlip { get; set; }
        public bool PhotocopyOfCertificates { get; set; }

        public bool PhotocopyOfNRIC { get; set; }
        public bool MaxSys_RulesAndRegulation { get; set; }

        public bool MaxSys_DoorAccessCard { get; set; }
        public bool EmailAccount { get; set; }
        public bool MaxSys_CompanyTShirt { get; set; }

        public bool TMS_DevelopmentStandardsBriefing { get; set; }

        public bool InternalTrainingAndTrainingMaterial { get; set; }

        public bool SignNDA { get; set; }
        public bool SignEmailAndInternetAgreement { get; set; }

        public bool TMS_EmployeeBadge { get; set; }

        public bool TMS_Safety { get; set; }
        public bool TMS_InternetAccess { get; set; }
        public bool TMS_VPN { get; set; }

        public bool TMS_EPortal_ELearning { get; set; }
        public bool TMS_TShirt_Cap_Tupperware { get; set; }
        public bool TMS_Locker { get; set; }
        public bool TMS_Laptop { get; set; }
        public bool MantorAndMantee { get; set; }
        public bool TMS_ServiceReportAccount { get; set; }
        public bool SMiT { get; set; }
        public DateTime? DateTimeCreated { get; set; }

        public string ReasonOfRejectOffer { get; set; }
    }
}