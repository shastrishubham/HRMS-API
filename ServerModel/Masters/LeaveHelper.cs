using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.ESS;
using ServerModel.Model.Masters;
using ServerModel.Repository;
using ServerModel.ServerModel.Ess;
using ServerModel.ServerModel.Masters.LeaveSetup;
using ServerModel.SqlAccess.ESS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Masters
{
    public class LeaveHelper
    {

        IRespository<EMP_LeaveReq> leaveRespository = null;
        IRespository<EMP_Info> employeeRespository = null;
        IRespository<MS_Leave> leaveMasterRespository = null;
        IRespository<EMP_Leaves> empLeaveRespository = null;
        EmployeeShiftHelper employeeShiftHelper;
        EmployeeHelper employeeHelper;



        EmployeeLeaveRequestRepository employeeLeaveRequestRepository;

        public LeaveHelper()
        {
            leaveRespository = new Repository<EMP_LeaveReq>();
            employeeRespository = new Repository<EMP_Info>();
            leaveMasterRespository = new Repository<MS_Leave>();
            empLeaveRespository = new Repository<EMP_Leaves>();


            employeeLeaveRequestRepository = new EmployeeLeaveRequestRepository();

            employeeShiftHelper = new EmployeeShiftHelper();
            employeeHelper = new EmployeeHelper();
        }

        public static IEssSetupAccess mEssSetupAccessT = new EssSetupAccessWrapper();


        public DataResult AddUpdateCreateEmployeeLeaveReqest(EmployeeLeaveRequestInformation employeeLeaveRequestInformation)
        {
            if (employeeLeaveRequestInformation.FromDate != null && employeeLeaveRequestInformation.FromDate != DateTime.MinValue
                && employeeLeaveRequestInformation.ToDate != null && employeeLeaveRequestInformation.ToDate != DateTime.MinValue)
            {
                bool overlapLeaves = IsLeaveOverlapping(employeeLeaveRequestInformation.CompId ?? Guid.Empty, employeeLeaveRequestInformation.EMP_Info_Id ?? Guid.Empty, employeeLeaveRequestInformation.FromDate.Value, employeeLeaveRequestInformation.ToDate.Value);

                if (overlapLeaves && employeeLeaveRequestInformation.Id == Guid.Empty)
                {
                    return new DataResult { ErrorMessage = "You already have leave applied for some or all of these dates..", IsSuccess = false };
                }

                // check employee can apply for this leave type
                List<LeaveInfo> leaves = LeaveSetupServer.GetLeavesByCompId(employeeLeaveRequestInformation.CompId ?? Guid.Empty);
                if (leaves == null)
                {
                    return new DataResult { ErrorMessage = "Leaves are not added for this company.", IsSuccess = false };
                }

                LeaveInfo leaveDetail = leaves.FirstOrDefault(x => x.Id == employeeLeaveRequestInformation.MS_Leave_Id);
                if (leaveDetail == null)
                {
                    return new DataResult { ErrorMessage = "This Type of Leave not able to find for this company.", IsSuccess = false };
                }

                if (leaveDetail.EffectiveAfterOnTypes == LeaveEffectiveAfterOnTypes.DateOfConfirmation)
                {
                    // check emp date of confirmation and if it's less than todays date then can apply for LOP.
                    EmployeeInformation employeeInformation = employeeHelper.GetEmployeeInformationById(employeeLeaveRequestInformation.EMP_Info_Id ?? Guid.Empty);
                    if (employeeInformation.ConfirmationDate == null)
                    {
                        string lopLeaveName = leaves.FirstOrDefault(x => x.LeaveType == LeaveTypes.Unpaid)?.LeaveName;
                        if (!string.IsNullOrEmpty(lopLeaveName))
                        {
                            // can not apply for this type of leave until date of confirmation, need to create other LOP leave type
                            return new DataResult { ErrorMessage = "You cannot apply for " + leaveDetail.LeaveName + " leave type until your confirmation date. Please select the " + lopLeaveName + " leave type instead.", IsSuccess = false };
                        }
                        else
                        {
                            return new DataResult { ErrorMessage = "You cannot apply for " + leaveDetail.LeaveName + " leave type until your confirmation date. Please Contact Help Desk.", IsSuccess = false };
                        }
                    }
                }

                // check for employee available leaves for this leave type
                List<EmployeeLeaves> empLeaves = mEssSetupAccessT.GetEmployeeAvailableLeavesByEmpId(employeeLeaveRequestInformation.EMP_Info_Id ?? Guid.Empty);
                if (empLeaves == null || empLeaves.Count == 0)
                {
                    return new DataResult { ErrorMessage = "Leaves are not added for this employee.", IsSuccess = false };
                }

                // get employee shift
                EmployeeShiftInformation employeeShift = employeeShiftHelper
                    .GetEmployeeShiftInformation(employeeLeaveRequestInformation.CompId ?? Guid.Empty, employeeLeaveRequestInformation.EMP_Info_Id ?? Guid.Empty);

                if (employeeShift != null)
                {
                    List<int> weeklyOffDayIds = ParseWeeklyOffDayIds(employeeShift.WeeklyOffId.ToString());
                    List<DayOfWeek> weeklyOffDays = weeklyOffDayIds.Select(i => (DayOfWeek)i).ToList();

                    if (employeeLeaveRequestInformation.LeaveFor == (int)LeaveFor.HalfDay)
                    {
                        employeeLeaveRequestInformation.ToDate = employeeLeaveRequestInformation.FromDate;
                        employeeLeaveRequestInformation.TotalDays = (decimal)0.5;
                    }
                    else
                    {
                        employeeLeaveRequestInformation.TotalDays = GetTotalLeaveDaysExcludingWeeklyOffs(employeeLeaveRequestInformation.FromDate.Value,
                            employeeLeaveRequestInformation.ToDate.Value, weeklyOffDays);
                    }

                    // 10 > (10 - 1) -> ok ; 10 > (10 - 10) -> ok ; 10 > (10 - 11) -> LOP
                    var availableLeave = empLeaves.FirstOrDefault(x => x.MS_Leave_Id == employeeLeaveRequestInformation.MS_Leave_Id)?.AvailableLeaves;
                    if (availableLeave > 0)
                    {
                        var empTotalDaysLeave = employeeLeaveRequestInformation.TotalDays ?? 0;
                        var remainingLeave = availableLeave - empTotalDaysLeave;

                        if (remainingLeave >= 0)
                        {
                            // employee can apply leave
                            return employeeLeaveRequestRepository.AddUpdateCreateEmployeeLeaveReqest(employeeLeaveRequestInformation);
                        }
                        else
                        {
                            // leave will be LOP
                            string lopLeaveName = leaves.FirstOrDefault(x => x.LeaveType == LeaveTypes.Unpaid)?.LeaveName;
                            if (!string.IsNullOrEmpty(lopLeaveName))
                            {
                                // can not apply for this type of leave until date of confirmation, need to create other LOP leave type
                                return new DataResult { ErrorMessage = "You cannot apply for " + leaveDetail.LeaveName + " leave type until your confirmation date. Please select the " + lopLeaveName + " leave type instead.", IsSuccess = false };
                            }
                            else
                            {
                                return new DataResult { ErrorMessage = "You cannot apply for " + leaveDetail.LeaveName + " leave type until your confirmation date. Please Contact Help Desk.", IsSuccess = false };
                            }
                        }
                    }
                }

                return new DataResult
                {
                    ErrorMessage = "Shift is not assign to selected employee.",
                    IsSuccess = false
                };
            }

            return new DataResult
            {
                ErrorMessage = "Something went wrong - From & To Date Validation Failed",
                IsSuccess = false
            };
        }

        public static List<int> ParseWeeklyOffDayIds(string weeklyOffDayIds)
        {
            if (string.IsNullOrWhiteSpace(weeklyOffDayIds))
                return new List<int>();

            if (weeklyOffDayIds.Contains(","))
            {
                return weeklyOffDayIds
                    .Split(',')
                    .Select(id => int.TryParse(id.Trim(), out int val) ? val : -1)
                    .Where(val => val >= 0 && val <= 6)
                    .ToList();
            }
            else
            {
                return int.TryParse(weeklyOffDayIds.Trim(), out int singleVal) && singleVal >= 0 && singleVal <= 6
                    ? new List<int> { singleVal }
                    : new List<int>();
            }
        }

        public int GetTotalLeaveDaysExcludingWeeklyOffs(DateTime leaveStartDate, DateTime leaveEndDate, List<DayOfWeek> weeklyOffs)
        {
            int leaveDays = 0;

            for (DateTime date = leaveStartDate.Date; date <= leaveEndDate.Date; date = date.AddDays(1))
            {
                if (!weeklyOffs.Contains(date.DayOfWeek))
                {
                    leaveDays++;
                }
            }

            return leaveDays;
        }

        public bool IsLeaveOverlapping(Guid compId, Guid employeeId, DateTime fromDate, DateTime toDate)
        {
            List<EmployeeLeaveRequestInformation> empLeaves = EssSetupServer.GetEmployeeLeaveRequestsByCompId(compId);

            DateTime fromDateOnly = fromDate.Date;
            DateTime toDateOnly = toDate.Date;

            
            return empLeaves.Any(lr =>
                            lr.EMP_Info_Id == employeeId &&
                            (lr.LeaveStatus == (int)LeaveStatusType.Pending || lr.LeaveStatus == (int)LeaveStatusType.Approved) &&
                            fromDateOnly <= lr.ToDate.Value.Date && toDateOnly >= lr.FromDate.Value.Date  // ✅ Overlap condition
                        );
        }


        public DataResult DeleteEmployeeLeaveRequest(EmployeeLeaveRequestInformation employeeLeaveRequestInformation)
        {
            return employeeLeaveRequestRepository.DeleteEmployeeLeaveRequest(employeeLeaveRequestInformation);
        }

        public IEnumerable<EmployeeLeaveRequestInformation> GetEmployeeLeaveReqByLeaveStatusAndCompId(Guid compId, LeaveStatusType leaveStatusType)
        {
            return mEssSetupAccessT.GetEmployeeLeaveReqByLeaveStatusAndCompId(compId, leaveStatusType);
        }

        public IEnumerable<EmployeeLeaveRequestInformation> GetEmployeeApprovedLeaveRequests(Guid compId)
        {
            int approvedLeaveStatus = (int)LeaveStatusType.Approved;

            var result = from leavesetup in leaveRespository.GetAll()
                         join empsetup in employeeRespository.GetAll() on leavesetup.EMP_Info_Id equals empsetup.Id
                         join empapprover in employeeRespository.GetAll() on leavesetup.IsApprovedBy equals empapprover.Id
                         join leavems in leaveMasterRespository.GetAll() on leavesetup.MS_Leave_Id equals leavems.Id
                         join empleave in empLeaveRespository.GetAll() on leavesetup.EMP_Info_Id equals empleave.EMP_Info_Id
                         where leavesetup.IsApprovedBy != null && leavesetup.LeaveStatus == approvedLeaveStatus
                         select new EmployeeLeaveRequestInformation
                         {
                             Id = leavesetup.Id,
                             EMP_Info_Id = leavesetup.EMP_Info_Id,
                             EmployeeName = empsetup.FullName,
                             MS_Leave_Id = leavesetup.MS_Leave_Id,
                             LeaveType = leavems.LeaveType.HasValue ? leavems.LeaveType.Value : 0,
                             FromDate = leavesetup.FromDate,
                             ToDate = leavesetup.ToDate,
                             LeaveFor = leavesetup.LeaveFor,
                             LeaveReason = leavesetup.LeaveReason,
                             IsApprovedBy = leavesetup.IsApprovedBy,
                             ApproverName = empapprover.FullName,
                             LeaveStatusType = leavesetup.LeaveStatus.HasValue ? leavesetup.LeaveStatus.Value : 0,
                             AvailableLeaves = empleave.AvailableLeaves ?? 0

                         };
            return result;

        }


        public decimal GetAvailableLeavesForEmployee(Guid employeeId)
        {
            var employeeLeaveDetails = empLeaveRespository.GetAll().Where(x => x.EMP_Info_Id == employeeId).FirstOrDefault();
            if (employeeLeaveDetails != null)
            {
                return employeeLeaveDetails.AvailableLeaves ?? 0;
            }
            return 0;
        }

    }
}
