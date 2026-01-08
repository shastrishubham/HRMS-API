using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Payroll
{
    public class PayrollInformation : PR_CRT
    {
        public string EmployeeName { get; set; }
        public string BranchName { get; set; }
        public decimal LoanDeductions { get; set; }

    }
}
