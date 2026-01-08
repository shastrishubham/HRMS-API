using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.BankSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.BankSetup
{
    public class BankSetupServer
    {
        #region Properties Interface

        public static IBankSetupAccess mBankSetupAccessT
            = new BankSetupAccessWrapper();

        #endregion


        public static List<BankInfo> GetBanksByCompId(Guid companyId)
        {
            return mBankSetupAccessT.GetBanksByCompId(companyId);
        }

        public static int UpsertBank(BankInfo bankInfo)
        {
            return mBankSetupAccessT.UpsertBank(bankInfo);
        }
    }
}
