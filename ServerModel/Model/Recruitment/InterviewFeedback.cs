using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Recruitment
{
    public class InterviewFeedback : Req_InterviewFedbck
    {
        public string DesignationName { get; set; }
        public string RequiredbranchName { get; set; }
        public string PreferredBranchName { get; set; }
        public Guid? HiringManager_Id { get; set; }
        public string HiringManagerName { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidateContact { get; set; }
        public Guid? InterviewTakenById { get; set; }
        public string InterviewTakenByName { get; set; }
        public decimal? YearOfExp { get; set; }

        public Guid Req_InterviewSch_Id { get; set; }
        public Guid FeedBackGivenEmpId { get; set; }
        public string FeedBackGivenEmpName { get; set; }
        public string InterviewerComment { get; set; }
        public Guid InterviewerId { get; set; }
        public string InterviewerName { get; set; }
        public int MS_InterviewRate_Id { get; set; }
        public string InterviewRate { get; set; }
        public string Feedback { get; set; }

        public DateTime InterviewDateTime { get; set; }
        public int Method { get; set; }
        public int InterviewStatus { get; set; }
        public int MS_Designation_Id { get; set; }
    }
}
