using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RecruitmentManagementSystem__Danny_.Models
{
    public class InterviewReport
    {
        public IEnumerable<Interview> Interviewees { get; set; }
        public IEnumerable<InterviewDetail> InterviewDetails { get; set; }
        public IEnumerable<InterviewerComment> InterviewerComments { get; set; }
        public IEnumerable<User> Interviewers { get; set; }
    }

}