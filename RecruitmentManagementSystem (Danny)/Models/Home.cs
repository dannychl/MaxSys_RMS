using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecruitmentManagementSystem__Danny_.Models
{
    [Table("dbo.Homes")]
    public class Home : global::System.Web.HttpApplication
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Candidate Name")]
        [Required]
        public string Name { get; set; }
        public string MethodUsed { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNum { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        [DisplayName("Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string Position { get; set; }
        public decimal ExpectedSalary { get; set; }
        public decimal CurrentSalary { get; set; }
        [DisplayName("Working Experience (Year)")]
        public int WorkingExperience { get; set; }
        [DisplayName("Remarks (Working Experience)")]
        public string WorkingExperienceRemarks { get; set; }
        [DisplayName("Resign Period (Month)")]
        public int ResignPeriod { get; set; }
        public int ProgrammingTest { get; set; }
        public int SQLTest { get; set; }
        [DisplayName("Test Remarks")]
        public string TestRemarks { get; set; }
        public string Status { get; set; }
    }
}