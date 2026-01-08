using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.BankSetup
{
    public interface IBankSetupAccess
    {
        int UpsertBank(BankInfo bankInfo);

        List<BankInfo> GetBanksByCompId(Guid companyId);
    }
}
