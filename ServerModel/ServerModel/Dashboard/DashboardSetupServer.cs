using ServerModel.Model.Dashboard;
using ServerModel.SqlAccess.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Dashboard
{
    public class DashboardSetupServer
    {
        #region Properties Interface

        public static IDashboardSetupAccess mDashboardSetupAccessT = new DashboardSetupAccessWrapper();

        #endregion


        public static DashboardResponseDto GetDashboard(Guid compId)
        {
            return mDashboardSetupAccessT.GetDashboard(compId);
        }
    }
}
