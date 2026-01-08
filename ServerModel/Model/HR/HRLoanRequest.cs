using ServerModel.Database;
using System;

namespace ServerModel.Model.HR
{
    public class HRLoanRequest : HR_LNReq
    {
        public string LNTypeName { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MaxAmount { get; set; }
        public bool IsMaxAmtManual { get; set; }
        public string SalaryHeadName { get; set; }
        public int MS_SLHead_Id { get; set; }
        public decimal Percentage { get; set; }
        public decimal ReqAmt { get; set; }
        public string LoanApplicantStatus { get; set; }
        public string LoanApplicantRemark { get; set; }
        public string LoanApplicant { get; set; }
        public Guid LoanApplicantId { get; set; }
        public string Approver { get; set; }
        public decimal TenureMonths { get; set; }


    }
}
