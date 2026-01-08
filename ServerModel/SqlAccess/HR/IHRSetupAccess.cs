using ServerModel.Model.Base;
using ServerModel.Model.HR;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.HR
{
    public interface IHRSetupAccess
    {
        Guid UpsertHRLoanRequest(HRLoanRequest hRLoanRequest);

        List<HRLoanRequest> GetHRLoanRequestsByStatus(Guid compId, LoanStatusTypes loanStatus) ;

        List<LoanRepaymentScheduleInfo> GetRePaymentScheduleByAmtIntesterAndTenure(decimal loanAmount, decimal interest, decimal tenure);

        LoanValidationInfo ValidateLoanRequestByEmpId(Guid empId, decimal loanAmount, int tenure, decimal interest);

        bool UpsertLoanDisbursementAndScheduleRepayment(LoanDisbursementInfo loanDisbursement);
    }
}
