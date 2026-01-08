using ServerModel.Model;
using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.ESS;
using ServerModel.Model.Masters;
using ServerModel.SqlAccess.ESS;
using ServerModel.SqlAccess.MasterSetup.ReimbursementTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerModel.ServerModel.Ess
{
    public class EssSetupServer
    {
        #region Properties Interface

        public static IEssSetupAccess mEssSetupAccessT = new EssSetupAccessWrapper();
        public static IReimbursementTypesAccess mReimbursementSetupAccessT = new ReimbursementTypesAccessWrapper();

        #endregion

        public static List<EmployeeLeaveRequestInformation> GetEmployeeLeaveRequestsByCompId(Guid compId)
        {
            return mEssSetupAccessT.GetEmployeeLeaveRequestsByCompId(compId);
        }

        public static EmployeeLeaveRequestInformation GetEmployeeLeaveRequestById(Guid requestId)
        {
            return mEssSetupAccessT.GetEmployeeLeaveRequestById(requestId);
        }

        public static bool AddUpdateEmployeeLeaves(EmployeeLeaves employeeLeave)
        {
            return mEssSetupAccessT.AddUpdateEmployeeLeaves(employeeLeave);
        }

        public static Model.DataResult UpdateEmployeeLeaveRequestStatusByIdAndStatus(Guid approverEmpId, Guid requestId, LeaveStatusType leaveStatusType)
        {
            bool isUpdated = false;
            List<EmployeeLeaves> empLeaves = null;
            EmployeeLeaveRequestInformation leaveRequestDetail = mEssSetupAccessT.GetEmployeeLeaveRequestById(requestId);

            if (leaveRequestDetail != null && leaveRequestDetail.LeaveStatus == (int)LeaveStatusType.Pending)
            {
                empLeaves = mEssSetupAccessT.GetEmployeeAvailableLeavesByEmpId(leaveRequestDetail.EMP_Info_Id ?? Guid.Empty);

                EmployeeLeaves employeeLeave = empLeaves.FirstOrDefault(x => x.MS_Leave_Id == leaveRequestDetail.MS_Leave_Id);

                // totalleave = 18, availableleave = 18, totaldays = 0.5
                // 18 > (18 - 0.5) = 18 > 17.05
                if (employeeLeave.TotalLeaves > (employeeLeave.AvailableLeaves - leaveRequestDetail.TotalDays))
                {
                    isUpdated = mEssSetupAccessT.UpdateEmployeeLeaveRequestStatusByIdAndStatus(approverEmpId, requestId, leaveStatusType);
                }
                else
                {
                    return new Model.DataResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Total Leaves are not exceeded than Available Leaves."
                    };
                }

                return new Model.DataResult
                {
                    IsSuccess = isUpdated,
                };
            }
            return new Model.DataResult
            {
                IsSuccess = false,
                ErrorMessage = "Leave Request is not able to find."
            };
        }

        #region Reimbursement
        public static List<ReimbursementClaims> GetReimbursementClaimsByCompId(Guid compId)
        {
            return mEssSetupAccessT.GetReimbursementClaimsByCompId(compId);
        }

        public static List<ReimbursementClaims> GetReimbursementClaimsByCompIdAndBranchId(Guid compId, int branchId)
        {
            return mEssSetupAccessT.GetReimbursementClaimsByCompIdAndBranchId(compId, branchId);
        }

        public static DataResult UpsertReimbursementClaims(ReimbursementClaims reimbursementClaim)
        {
            // check if emp created already same reimbursement type claim
            // check frequency of reimb type, e.g for monthly he can apply only one, yearly but exceeds limit then not to apply
            // employee can not edit if paid date, approve date is exists.

            if (reimbursementClaim.Id != Guid.Empty)
            {
                // edit reimbursement
                List<ReimbursementClaims> empReimbursementPendingClaims = mEssSetupAccessT.GetReimbursementClaimsByEmployeeId(reimbursementClaim.EMP_Info_Id.Value)
                    .Where(x => x.ApprovedDate == null && x.PaidDate == null && x.Status == "Open").ToList();

                bool canEditable = empReimbursementPendingClaims.Any(x => x.Id == reimbursementClaim.Id);

                if (canEditable)
                {
                    Guid id = mEssSetupAccessT.UpsertReimbursementClaims(reimbursementClaim);

                    if (id != Guid.Empty)
                    {
                        return new DataResult
                        {
                            IsSuccess = true
                        };
                    }
                }
                else
                {
                    return new DataResult { IsSuccess = false, ErrorMessage = "A claim of this type has already been approved." };
                }
            }
            else
            {
                // new reimbursement
                if (reimbursementClaim.CompId.HasValue && reimbursementClaim.EMP_Info_Id.HasValue && reimbursementClaim.MS_Reim_Types_Id.HasValue)
                {
                    int claimType = reimbursementClaim.MS_Reim_Types_Id.Value;

                    ReimbursementTypes reimbursementTypeDetail = mReimbursementSetupAccessT.GetReimbursementTypesByCompId(reimbursementClaim.CompId.Value)
                        .FirstOrDefault(x => x.Id == claimType);

                    List<ReimbursementClaims> empReimbursementClaims = mEssSetupAccessT.GetReimbursementClaimsByEmployeeId(reimbursementClaim.EMP_Info_Id.Value)
                        .Where(x => x.MS_Reim_Types_Id == claimType).ToList();

                    if (empReimbursementClaims.Any() && empReimbursementClaims.Count > 0)
                    {
                        if (reimbursementTypeDetail.Frequency == "Monthly")
                        {
                            // check claim date, emp should not be create in same claim date month
                            List<ReimbursementClaims> empReimbursementOpenClaims = empReimbursementClaims
                                .Where(x => x.ApprovedDate == null && x.PaidDate == null && x.Status == "Open").ToList();
                            if (empReimbursementOpenClaims != null && empReimbursementClaims.Any())
                            {
                                int claimMonth = reimbursementClaim.ClaimDate.Value.Month;
                                if (empReimbursementClaims.Any(x => x.ClaimDate.Value.Month == claimMonth))
                                {
                                    return new DataResult { IsSuccess = false, ErrorMessage = "A claim of this type has already been created for this month. Please edit the existing one instead." };
                                }
                                else
                                {
                                    Guid id = mEssSetupAccessT.UpsertReimbursementClaims(reimbursementClaim);
                                    return new DataResult { IsSuccess = true };
                                }
                            }
                            else
                            {
                                // claim is closed
                                return new DataResult { IsSuccess = false, ErrorMessage = "You’ve already submitted this type of claim for this month, and it has been approved. You cannot submit another one." };
                            }

                        }
                        else if (reimbursementTypeDetail.Frequency == "Yearly")
                        {
                            // check claim date, emp should not be create in same claim year
                            List<ReimbursementClaims> empReimbursementOpenClaims = empReimbursementClaims
                                .Where(x => x.ApprovedDate == null && x.PaidDate == null && x.Status == "Open").ToList();
                            if (empReimbursementOpenClaims != null)
                            {
                                int claimYear = reimbursementClaim.ClaimDate.Value.Year;
                                if (empReimbursementOpenClaims.Any(x=>x.ClaimDate.Value.Year == claimYear))
                                {
                                    return new DataResult { IsSuccess = false, ErrorMessage = "A claim of this type has already been created for this year. Please edit the existing one instead." };
                                }
                                else
                                {
                                    Guid id = mEssSetupAccessT.UpsertReimbursementClaims(reimbursementClaim);
                                    return new DataResult { IsSuccess = true };
                                }
                            }
                            else
                            {
                                // claim is closed
                                return new DataResult { IsSuccess = false, ErrorMessage = "You’ve already submitted this type of claim for this year, and it has been approved. You cannot submit another one." };
                            }
                        }
                        else if (reimbursementTypeDetail.Frequency == "One-Time")
                        {
                            // check claim, if it's already created no need to create again
                            ReimbursementClaims empReimbursementOpenClaims = empReimbursementClaims
                               .FirstOrDefault(x => x.ApprovedDate == null && x.PaidDate == null && x.Status == "Open");
                            if (empReimbursementOpenClaims != null)
                            {
                                // cannot create same type claim in multiple times, as it's already being created, either edit this.
                                return new DataResult { IsSuccess = false, ErrorMessage = "You cannot create multiple claims of the same type. Please edit the existing one instead." };
                            }
                            else
                            {
                                // claim is closed
                                // you already submitted claim for this one time, so cannot submit this types of reimbursement claim.
                                return new DataResult { IsSuccess = false, ErrorMessage = "You’ve already submitted this type of claim for one-time, and it has been approved. You cannot submit another one." };
                            }
                        }
                    }
                    else
                    {
                        Guid id = mEssSetupAccessT.UpsertReimbursementClaims(reimbursementClaim);
                        return new DataResult
                        {
                            IsSuccess = true
                        };
                    }
                    #endregion
                }
            }
            return new DataResult { IsSuccess = false, ErrorMessage = "Something went wrong" };
        }

        public static DataResult ApprovedReimbursementClaim(ReimbursementApproverModel reimbursementApproverModel)
        {
            Guid id = mEssSetupAccessT.ApprovedReimbursementClaim(reimbursementApproverModel);
            if (id == Guid.Empty)
                return new DataResult { IsSuccess = false };
            else
                return new DataResult { IsSuccess = true };
        }


        #region Loan

        public static Guid UpsertEmployeeLoanRequest(EmpLoanRequest empLoanRequest)
        {
            return mEssSetupAccessT.UpsertEmployeeLoanRequest(empLoanRequest);
        }

        public static List<EmpLoanRequest> GetEmployeeLoanRequestsByCompId(Guid compId)
        {
            return mEssSetupAccessT.GetEmployeeLoanRequestsByCompId(compId);
        }

        #endregion
    }
}
