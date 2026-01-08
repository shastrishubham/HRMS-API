using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.BankBranchSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.BankBranchSetup
{
    public class BankBranchServer
    {
        #region Properties Interface

        public static IBankBranchSetupAccess mBankBranchSetupAccessT
            = new BankBranchSetupAccessWrapper();

        #endregion


        public static List<BankBranches> GetBranchesByBankAndCompId(Guid companyId, int bankId)
        {
            return mBankBranchSetupAccessT.GetBranchesByBankAndCompId(companyId, bankId);
        }

        public static List<BankBranches> GetBankBranchesByCompId(Guid companyId)
        {
            return mBankBranchSetupAccessT.GetBankBranchesByCompId(companyId);
        }

        public static int UpsertBankBranch(BankBranches BankBranch)
        {
            return mBankBranchSetupAccessT.UpsertBankBranch(BankBranch);
        }
    }
}
