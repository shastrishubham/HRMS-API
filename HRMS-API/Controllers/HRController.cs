using ServerModel.Data;
using ServerModel.Model.Base;
using ServerModel.Model.HR;
using ServerModel.ServerModel.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HRController : ApiController
    {
        public HRController()
        {
            DbContext.ConnectionString = WebApiConfig.ConnectionString;
        }


        #region Loan Request

        [AllowAnonymous]
        [HttpPost]
        [Route("api/HR/UpsertHRLoanRequest")]
        public Guid UpsertEmployeeLoanRequest(HRLoanRequest loanRequest)
        {
            return HRSetupServer.UpsertHRLoanRequest(loanRequest);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/HR/GetHRLoanRequestsByStatus")]
        public List<HRLoanRequest> GetHRLoanRequestsByStatus(Guid compId, LoanStatusTypes statusType)
        {
            return HRSetupServer.GetHRLoanRequestsByStatus(compId, statusType);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/HR/GetRePaymentScheduleByAmtIntesterAndTenure")]
        public List<LoanRepaymentScheduleInfo> GetRePaymentScheduleByAmtIntesterAndTenure(decimal loanAmount, decimal interest, decimal tenure)
        {
            return HRSetupServer.GetRePaymentScheduleByAmtIntesterAndTenure(loanAmount, interest, tenure);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/HR/ValidateLoanRequestByEmpId")]
        public LoanValidationInfo ValidateLoanRequestByEmpId(Guid empId, decimal loanAmount, int tenure, decimal interest)
        {
            return HRSetupServer.ValidateLoanRequestByEmpId(empId, loanAmount, tenure, interest);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/HR/UpsertLoanDisbursementAndScheduleRepayment")]
        public bool UpsertLoanDisbursementAndScheduleRepayment(LoanDisbursementInfo loanDisbursement)
        {
            return HRSetupServer.UpsertLoanDisbursementAndScheduleRepayment(loanDisbursement);
        }

        #endregion
    }
}
