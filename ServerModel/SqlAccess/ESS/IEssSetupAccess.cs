using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.ESS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.ESS
{
    public interface IEssSetupAccess
    {
        List<EmployeeLeaveRequestInformation> GetEmployeeLeaveRequestsByCompId(Guid compId);

        EmployeeLeaveRequestInformation GetEmployeeLeaveRequestById(Guid requestId);

        List<EmployeeLeaves> GetEmployeeAvailableLeavesByEmpId(Guid employeeId);

        bool AddUpdateEmployeeLeaves(EmployeeLeaves employeeLeave);

        List<EmployeeLeaveRequestInformation> GetEmployeeLeaveReqByLeaveStatusAndCompId(Guid compId, LeaveStatusType leaveStatus);

        bool UpdateEmployeeLeaveRequestStatusByIdAndStatus(Guid approverEmpId ,Guid requestId, LeaveStatusType leaveStatusType);

        Guid UpsertReimbursementClaims(ReimbursementClaims reimbursementClaim);

        List<ReimbursementClaims> GetReimbursementClaimsByCompId(Guid compId);

        List<ReimbursementClaims> GetReimbursementClaimsByEmployeeId(Guid employeeId);

        List<ReimbursementClaims> GetReimbursementClaimsByCompIdAndBranchId(Guid compId, int branchId);

        Guid ApprovedReimbursementClaim(ReimbursementApproverModel reimbursementApproverModel);

        #region Loan Module

        Guid UpsertEmployeeLoanRequest(EmpLoanRequest empLoanRequest);

        List<EmpLoanRequest> GetEmployeeLoanRequestsByCompId(Guid compId);

        #endregion
    }
}
