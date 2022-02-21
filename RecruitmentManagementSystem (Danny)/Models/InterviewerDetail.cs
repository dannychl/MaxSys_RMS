using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitmentManagementSystem__Danny_.Models
{
    public class InterviewerDetail
    {
        int Id { get; set; }
        int IntervieweeId { get; set; }
        int InterviewerUserId { get; set; }
        DateTime InterviewTime { get; set; }
        DateTime InterviewDate { get; set; }
        string InterviewRemarks { get; set; }
        string InterviewResult { get; set; }

    }
}