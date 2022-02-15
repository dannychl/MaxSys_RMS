using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecruitmentManagementSystem__Danny_.Models
{
    public class Home: global::System.Web.HttpApplication
    {
            public int Id { get; set; }
            [DisplayName("Candidate Name")]
            public string Name { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            [DisplayName("Date Of Birth")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
            public DateTime DateOfBirth { get; set; }
            public string Position { get; set; }
            public decimal ExpectedSalary { get; set; }
    }
}