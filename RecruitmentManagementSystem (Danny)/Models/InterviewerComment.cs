using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecruitmentManagementSystem__Danny_.Models
{
    [Table("dbo.InterviewerComment")]
    public class InterviewerComment
    {
        [Key]
        public int Id { get; set; }
        public int InterviewerId { get; set; }
        public int InterviewDetailId { get; set; }
        public string InterviewRemarks { get; set; }
        public string InterviewResult { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

    }
}