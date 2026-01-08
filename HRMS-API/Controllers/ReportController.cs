using ServerModel.Data;
using ServerModel.Model.Reports;
using ServerModel.ServerModel.Report;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        public ReportController()
        {
            DbContext.ConnectionString = WebApiConfig.ConnectionString;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Report/BranchWiseEmpReports")]
        public List<BranchWiseEmpReportInfo> BranchWiseEmpReports(Guid compId, int branchId)
        {
            return ReportInfoServer.BranchWiseEmpReports(compId, branchId);
        }
    }
}
