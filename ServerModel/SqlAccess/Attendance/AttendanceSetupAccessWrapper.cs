using ServerModel.Model.Punch;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.Attendance
{
    public class AttendanceSetupAccessWrapper : IAttendanceSetupAccess
    {
        public List<EmployeeAttendanceInfo> GetEmployeePunchesById(Guid employeeId, int month, int year)
        {
            return AttendanceSetupAccess.GetEmployeePunchesById(employeeId, month, year);
        }
    }
}
