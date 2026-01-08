using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeeLeaveSetupInformation : EMP_Leaves
    {
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
    }
}
