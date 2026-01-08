using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.LoanSetup
{
    public class LoanSetupAccessWrapper : ILoanSetupAccess
    {
        public List<LoanSetupInfo> GetLoanSetups(Guid compId)
        {
            return LoanSetupAccess.GetLoanSetups(compId);
        }

        public int UpsertLoanTypes(LoanSetupInfo loanSetup)
        {
            return LoanSetupAccess.UpsertLoanTypes(loanSetup);
        }
    }
}
