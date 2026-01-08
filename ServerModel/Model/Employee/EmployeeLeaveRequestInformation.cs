using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeeLeaveRequestInformation : EMP_LeaveReq
    {
        public string EmployeeName { get; set; }
        public int LeaveType { get; set; }
        public string ApproverName { get; set; }
        public decimal AvailableLeaves { get; set; }
        public int LeaveStatusType { get; set; }
        public string LeaveName { get; set; }
        public string Docs { get; set; }
        public Guid EMP_Leaves_Id { get; set; }
        public string Approver { get; set; }
    }
}
