using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.ShiftSetup
{
    public interface IShiftSetupAccess
    {
        int UpsertShift(ShiftInfo shiftInfo);

        List<ShiftInfo> GetShiftDetailsByBranchIdAndCompId(Guid companyId, int branchId);

        List<ShiftInfo> GetShiftDetailsByCompId(Guid companyId);
    }
}
