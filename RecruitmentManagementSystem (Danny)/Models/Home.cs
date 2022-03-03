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
    public class Home //: global::System.Web.HttpApplication
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Candidate Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        public string MethodUsed { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10,11})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNum { get; set; }

        [Required]
        public string Gender { get; set; }

        public int Age { get; set; }

        [Required]
        [DisplayName("Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public decimal ExpectedSalary { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public decimal CurrentSalary { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        [DisplayName("Working Experience (Year)")]
        public int WorkingExperience { get; set; }

        [DisplayName("Remarks (Working Experience)")]
        public string WorkingExperienceRemarks { get; set; }

        [DisplayName("Resign Period (Month)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int ResignPeriod { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int ProgrammingTest { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int SQLTest { get; set; }
        
        [DisplayName("Test Remarks")]
        public string TestRemarks { get; set; }
        
        public string Status { get; set; }
    }
}