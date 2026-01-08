using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.LeaveSetup
{
    public interface ILeaveSetupInfo
    {
        int UpsertLeave(LeaveInfo leaveInfo);

        List<LeaveInfo> GetLeavesByCompId(Guid companyId);
    }
}
