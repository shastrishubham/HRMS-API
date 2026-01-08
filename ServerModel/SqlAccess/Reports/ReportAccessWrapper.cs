using ServerModel.Model.Reports;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.Reports
{
    public class ReportAccessWrapper : IReportAccess
    {
        public List<BranchWiseEmpReportInfo> BranchWiseEmpReports(Guid compId, int branchId)
        {
            return ReportAccess.branchWiseEmpReports(compId, branchId);
        }
    }
}
