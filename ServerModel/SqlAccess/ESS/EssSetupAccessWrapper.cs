using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.ESS;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.ESS
{
    public class EssSetupAccessWrapper : IEssSetupAccess
    {
        public bool AddUpdateEmployeeLeaves(EmployeeLeaves employeeLeave)
        {
            return EssSetupAccess.AddUpdateEmployeeLeaves(employeeLeave);
        }

        public List<EmployeeLeaves> GetEmployeeAvailableLeavesByEmpId(Guid employeeId)
        {
            return EssSetupAccess.GetEmployeeAvailableLeavesByEmpId(employeeId);
        }

        public List<EmployeeLeaveRequestInformation> GetEmployeeLeaveReqByLeaveStatusAndCompId(Guid compId, LeaveStatusType leaveStatus)
        {
            return EssSetupAccess.GetEmployeeLeaveReqByLeaveStatusAndCompId(compId, leaveStatus);
        }

        public EmployeeLeaveRequestInformation GetEmployeeLeaveRequestById(Guid requestId)
        {
            return EssSetupAccess.GetEmployeeLeaveRequestById(requestId);
        }

        public List<EmployeeLeaveRequestInformation> GetEmployeeLeaveRequestsByCompId(Guid compId)
        {
            return EssSetupAccess.GetEmployeeLeaveRequestsByCompId(compId);
        }

        public bool UpdateEmployeeLeaveRequestStatusByIdAndStatus(Guid approverEmpId, Guid requestId, LeaveStatusType leaveStatusType)
        {
            return EssSetupAccess.UpdateEmployeeLeaveRequestStatusByIdAndStatus(approverEmpId, requestId, leaveStatusType);
        }

        #region Reimbursement

        public List<ReimbursementClaims> GetReimbursementClaimsByCompId(Guid compId)
        {
            return EssSetupAccess.GetReimbursementClaimsByCompId(compId);
        }

        public List<ReimbursementClaims> GetReimbursementClaimsByCompIdAndBranchId(Guid compId, int branchId)
        {
            return EssSetupAccess.GetReimbursementClaimsByCompIdAndBranchId(compId, branchId);
        }

        public List<ReimbursementClaims> GetReimbursementClaimsByEmployeeId(Guid employeeId)
        {
            return EssSetupAccess.GetReimbursementClaimsByEmployeeId(employeeId);
        }

        public Guid UpsertReimbursementClaims(ReimbursementClaims reimbursementClaim)
        {
            return EssSetupAccess.UpsertReimbursementClaims(reimbursementClaim);
        }

        public Guid ApprovedReimbursementClaim(ReimbursementApproverModel reimbursementApproverModel)
        {
            return EssSetupAccess.ApprovedReimbursementClaim(reimbursementApproverModel);
        }

        #endregion

        #region Loan

        public Guid UpsertEmployeeLoanRequest(EmpLoanRequest empLoanRequest)
        {
            return EssSetupAccess.UpsertEmployeeLoanRequest(empLoanRequest);
        }

        public List<EmpLoanRequest> GetEmployeeLoanRequestsByCompId(Guid compId)
        {
            return EssSetupAccess.GetEmployeeLoanRequestsByCompId(compId);
        }

        #endregion
    }
}
