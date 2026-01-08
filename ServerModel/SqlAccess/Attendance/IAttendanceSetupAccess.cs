using ServerModel.Model.Punch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.Attendance
{
    public interface IAttendanceSetupAccess
    {
        List<EmployeeAttendanceInfo> GetEmployeePunchesById(Guid employeeId, int month, int year);
    }
}
