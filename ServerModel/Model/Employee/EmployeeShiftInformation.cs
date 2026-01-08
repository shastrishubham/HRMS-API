using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeeShiftInformation : EMP_Shift
    {
        public string EmployeeFullName { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeMiddleName { get; set; }
        public string EmployeeLastName { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public int? EmployeeBranchId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public List<int> WeeklyOffId { get; set; }

        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
    }
}
