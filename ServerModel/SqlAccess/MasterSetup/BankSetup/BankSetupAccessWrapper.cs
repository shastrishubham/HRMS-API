using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Model.Masters;

namespace ServerModel.SqlAccess.MasterSetup.BankSetup
{
    public class BankSetupAccessWrapper : IBankSetupAccess
    {
        public List<BankInfo> GetBanksByCompId(Guid companyId)
        {
            return BankSetupAccess.GetBanksByCompId(companyId);
        }

        public int UpsertBank(BankInfo bankInfo)
        {
            return BankSetupAccess.UpsertBank(bankInfo);
        }
    }
}
