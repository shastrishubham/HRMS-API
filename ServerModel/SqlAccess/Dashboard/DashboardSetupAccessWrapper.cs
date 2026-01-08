using ServerModel.Model.Dashboard;
using System;

namespace ServerModel.SqlAccess.Dashboard
{
    public class DashboardSetupAccessWrapper : IDashboardSetupAccess
    {
        public DashboardResponseDto GetDashboard(Guid compId)
        {
            return DashboardSetupAccess.GetDashboard(compId);
        }
    }
}
