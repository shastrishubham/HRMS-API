using ServerModel.Model.Base;
using ServerModel.Model.HR;
using ServerModel.SqlAccess.HR;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.HR
{
    public class HRSetupServer
    {
        #region Properties Interface

        public static IHRSetupAccess mHRSetupAccessT
            = new HRSetupAccessWrapper();

        #endregion


        public static List<HRLoanRequest> GetHRLoanRequestsByStatus(Guid compId, LoanStatusTypes statusType)
        {
            return mHRSetupAccessT.GetHRLoanRequestsByStatus(compId, statusType);
        }

        public static List<LoanRepaymentScheduleInfo> GetRePaymentScheduleByAmtIntesterAndTenure(decimal loanAmount, decimal interest, decimal tenure)
        {
            return mHRSetupAccessT.GetRePaymentScheduleByAmtIntesterAndTenure(loanAmount, interest, tenure);
        }

        public static LoanValidationInfo ValidateLoanRequestByEmpId(Guid empId, decimal loanAmount, int tenure, decimal interest)
        {
            return mHRSetupAccessT.ValidateLoanRequestByEmpId(empId, loanAmount, tenure, interest);
        }

        public static bool UpsertLoanDisbursementAndScheduleRepayment(LoanDisbursementInfo loanDisbursement)
        {
            return mHRSetupAccessT.UpsertLoanDisbursementAndScheduleRepayment(loanDisbursement);
        }

        public static Guid UpsertHRLoanRequest(HRLoanRequest loanSetup)
        {
            return mHRSetupAccessT.UpsertHRLoanRequest(loanSetup);
        }


    }
}
