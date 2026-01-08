using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Model.Masters;

namespace ServerModel.SqlAccess.MasterSetup.ShiftSetup
{
    public class ShiftSetupAccessWrapper : IShiftSetupAccess
    {
        public List<ShiftInfo> GetShiftDetailsByBranchIdAndCompId(Guid companyId, int branchId)
        {
            return ShiftSetupAccess.GetShiftDetailsByBranchIdAndCompId(companyId, branchId);
        }

        public List<ShiftInfo> GetShiftDetailsByCompId(Guid companyId)
        {
            return ShiftSetupAccess.GetShiftDetailsByCompId(companyId);
        }

        public int UpsertShift(ShiftInfo shiftInfo)
        {
            return ShiftSetupAccess.UpsertShift(shiftInfo);
        }
    }
}
