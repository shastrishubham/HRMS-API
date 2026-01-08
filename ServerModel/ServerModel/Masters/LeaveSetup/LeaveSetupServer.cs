using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.LeaveSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.LeaveSetup
{
    public class LeaveSetupServer
    {
        #region Properties Interface

        public static ILeaveSetupInfo mLeaveSetupAccessT
            = new LeaveSetupAccessWrapper();

        #endregion


        public static List<LeaveInfo> GetLeavesByCompId(Guid companyId)
        {
            return mLeaveSetupAccessT.GetLeavesByCompId(companyId);
        }

        public static int UpsertLeave(LeaveInfo leaveInfo)
        {
            return mLeaveSetupAccessT.UpsertLeave(leaveInfo);
        }
    }
}
