using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.BankBranchSetup
{
    public class BankBranchSetupAccessWrapper : IBankBranchSetupAccess
    {
        public List<BankBranches> GetBankBranchesByCompId(Guid companyId)
        {
            return BankBranchSetupAccess.GetBankBranchesByCompId(companyId);
        }

        public List<BankBranches> GetBranchesByBankAndCompId(Guid companyId, int bankId)
        {
            return BankBranchSetupAccess.GetBranchesByBankAndCompId(companyId, bankId);
        }
    
        public int UpsertBankBranch(BankBranches bankBranch)
        {
            return BankBranchSetupAccess.UpsertBankBranch(bankBranch);
        }
    }
}
