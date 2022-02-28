using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecruitmentManagementSystem__Danny_.Models
{
    [Table("dbo.Interview_Detail")]
    public class InterviewDetail
    {
        [Key]
        public int Id { get; set; }
        public int IntervieweeId { get; set; }
        public int InterviewerUserId { get; set; }
        /*[DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]*/
        public string InterviewTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InterviewDate { get; set; }
        public string InterviewRemarks { get; set; }
        public string InterviewResult { get; set; }
        public int InterviewProgress { get; set; }

    }
}