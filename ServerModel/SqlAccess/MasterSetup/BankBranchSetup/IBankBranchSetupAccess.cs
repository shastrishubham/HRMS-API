using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.BankBranchSetup
{
    public interface IBankBranchSetupAccess
    {
        int UpsertBankBranch(BankBranches bankBraches);

        List<BankBranches> GetBranchesByBankAndCompId(Guid companyId, int bankId);

        List<BankBranches> GetBankBranchesByCompId(Guid companyId);
    }
}
