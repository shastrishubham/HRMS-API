using ServerModel.Model.Reports;
using ServerModel.SqlAccess.Reports;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Report
{
    public class ReportInfoServer
    {
        #region Properties Interface

        public static IReportAccess mReportSetupAccessT = new ReportAccessWrapper();

        #endregion

        public static List<BranchWiseEmpReportInfo> BranchWiseEmpReports(Guid compId, int branchId)
        {
            return mReportSetupAccessT.BranchWiseEmpReports(compId, branchId);
        }
    }
}
