using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecruitmentManagementSystem__Danny_.Models
{
    [Table("dbo.Interviewee")]
    public class Interview
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Interviewee Name")]
        public string IntervieweeName { get; set; }
        [DisplayName("Interviewee Status")]
        public string IntervieweeStatus { get; set; }
        [DisplayName("First Interview")]
        public string FirstInterviewerStatus { get; set; }
        [DisplayName("Second Interview")]
        public string SecondInterviewerStatus { get; set; }
        public string IntervieweeResumeLink { get; set; }
        public int CandidatesId { get; set; }
    }
}