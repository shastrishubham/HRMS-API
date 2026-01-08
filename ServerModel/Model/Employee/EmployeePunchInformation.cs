using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeePunchInformation : EMP_Punch
    {
        public Guid EmpId { get; set; }
        public string EmpName { get; set; }
        public string DesignationName { get; set; }
        public string BranchName { get; set; }
        public string ShiftName { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime EndTo { get; set; }
        public bool IsPermanentShift { get; set; }
        public TimeSpan Intime { get; set; }
        public TimeSpan OutTime { get; set; }
    }
}
