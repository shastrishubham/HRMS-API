using ServerModel.Database;
using System;
using System.Collections.Generic;

namespace ServerModel.Model.Recruitment
{
    public class EmployeeGeneratedDocument : EMP_GenDocs
    {
        public List<EmployeeOfferLetterSalaryInfo> SalaryComponents { get; set; }

        public string CompanyName { get; set; }
        public string CompanyContact { get; set; }
        public string Website { get; set; }
        public string CompanyAddress { get; set; }
        public string CompCity { get; set; }
        public string CompState { get; set; }
        public string CompCountry { get; set; }
        public string Pincode { get; set; }
        public string CandidateFullName { get; set; }
        public string CandidateAddress { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public string ReportingManager { get; set; }


        public string DocName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string InterviewerName { get; set; }

        public Guid InterviewScheduledId { get; set; }
        public int InterviewStatusId { get; set; }
    }
}
