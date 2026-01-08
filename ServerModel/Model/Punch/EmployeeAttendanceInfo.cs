using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Punch
{
    public class EmployeeAttendanceInfo : EMP_Punch
    {
        public DateTime WorkDate { get; set; }
        public TimeSpan PunchIn { get; set; }
        public TimeSpan PunchOut { get; set; }
        public string LeaveType { get; set; }
        public string PublicHolidayName { get; set; }
    }
}

