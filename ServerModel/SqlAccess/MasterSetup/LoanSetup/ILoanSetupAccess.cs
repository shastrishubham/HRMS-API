using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.LoanSetup
{
    public interface ILoanSetupAccess
    {
        int UpsertLoanTypes(LoanSetupInfo loanSetup);

        List<LoanSetupInfo> GetLoanSetups(Guid compId);
    }
}
