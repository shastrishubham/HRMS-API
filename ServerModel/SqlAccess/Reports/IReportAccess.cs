using ServerModel.Model.Reports;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.Reports
{
    public interface IReportAccess
    {
        List<BranchWiseEmpReportInfo> BranchWiseEmpReports(Guid compId, int branchId);
    }
}
