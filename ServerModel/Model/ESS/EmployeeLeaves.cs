using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.ESS
{
    public class EmployeeLeaves : EMP_Leaves
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveName { get; set; }
    }
}
