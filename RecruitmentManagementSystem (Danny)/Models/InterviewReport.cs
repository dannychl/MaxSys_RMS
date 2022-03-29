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
        public Interview Interviewee { get; set; }
        public InterviewDetail InterviewDetail { get; set; }
        public InterviewerComment InterviewerComment { get; set; }
        public User Interviewer { get; set; }

        public Candidate Candidate { get; set; }
    }

}