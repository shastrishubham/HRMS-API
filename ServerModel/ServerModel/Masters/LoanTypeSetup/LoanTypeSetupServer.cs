using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.LoanSetup;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Masters.LoanTypeSetup
{
    public class LoanTypeSetupServer
    {
        #region Properties Interface

        public static ILoanSetupAccess mLoanTypeSetupAccessT
            = new LoanSetupAccessWrapper();

        #endregion


        public static List<LoanSetupInfo> GetLoanSetups(Guid compId)
        {
            return mLoanTypeSetupAccessT.GetLoanSetups(compId);
        }

        public static int UpsertLoanTypes(LoanSetupInfo loanSetup)
        {
            return mLoanTypeSetupAccessT.UpsertLoanTypes(loanSetup);
        }
    }
}
