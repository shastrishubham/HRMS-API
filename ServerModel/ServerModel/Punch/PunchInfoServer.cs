using ServerModel.Model.Punch;
using ServerModel.SqlAccess.Attendance;
using ServerModel.SqlAccess.Punch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Punch
{
    public class PunchInfoServer
    {
        #region Properties Interface

        public static IAttendanceSetupAccess mAttendanceAccessT = new AttendanceSetupAccessWrapper();

        #endregion

        public int AddPunchInTime(PunchInOut punchInOut)
        {
            return PunchSetupAccess.AddPunchInTime(punchInOut);
        }

        public List<EmployeeAttendanceInfo> GetEmployeePunchesById(Guid employeeId, int month, int year)
        {
            return mAttendanceAccessT.GetEmployeePunchesById(employeeId, month, year);
        }
        //public List<EmployeeAttendanceInfo> GetPresentEmpByBranch(DateTime attendaceDate, int branchId)
        //{
        //    return PunchSetupAccess.GetPresentEmpByBranch(attendaceDate, branchId);
        //}


    }
}
