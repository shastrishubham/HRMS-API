using ServerModel.Model.Dashboard;
using System;

namespace ServerModel.SqlAccess.Dashboard
{
    public interface IDashboardSetupAccess
    {
        DashboardResponseDto GetDashboard(Guid compId);
    }
}
