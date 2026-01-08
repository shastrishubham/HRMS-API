using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Model.Masters;

namespace ServerModel.SqlAccess.MasterSetup.LeaveSetup
{
    public class LeaveSetupAccessWrapper : ILeaveSetupInfo
    {
        public List<LeaveInfo> GetLeavesByCompId(Guid companyId)
        {
            return LeaveSetupAccess.GetLeavesByCompId(companyId);
        }

        public int UpsertLeave(LeaveInfo leaveInfo)
        {
            return LeaveSetupAccess.UpsertLeave(leaveInfo);
        }
    }
}
