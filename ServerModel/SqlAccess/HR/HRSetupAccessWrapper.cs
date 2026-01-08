using ServerModel.Model.Base;
using ServerModel.Model.HR;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.HR
{
    public class HRSetupAccessWrapper : IHRSetupAccess
    {
        public List<HRLoanRequest> GetHRLoanRequestsByStatus(Guid compId, LoanStatusTypes loanStatus)
        {
            return HRSetupAccess.GetHRLoanRequestsByStatus(compId, loanStatus);
        }

        public List<LoanRepaymentScheduleInfo> GetRePaymentScheduleByAmtIntesterAndTenure(decimal loanAmount, decimal interest, decimal tenure)
        {
            return HRSetupAccess.GetRePaymentScheduleByAmtIntesterAndTenure(loanAmount, interest, tenure);
        }

        public Guid UpsertHRLoanRequest(HRLoanRequest hRLoanRequest)
        {
            return HRSetupAccess.UpsertHRLoanRequest(hRLoanRequest);
        }

        public bool UpsertLoanDisbursementAndScheduleRepayment(LoanDisbursementInfo loanDisbursement)
        {
            return HRSetupAccess.UpsertLoanDisbursementAndScheduleRepayment(loanDisbursement);
        }

        public LoanValidationInfo ValidateLoanRequestByEmpId(Guid empId, decimal loanAmount, int tenure, decimal interest)
        {
            return HRSetupAccess.ValidateLoanRequestByEmpId(empId, loanAmount, tenure, interest);
        }
    }
}
