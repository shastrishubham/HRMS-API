using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Payroll
{
    public class EmployeePayrollInformation : PR_EMP_SL
    {
        public string EmpId { get; set; }
        public string BankName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string FullName { get; set; }
        public string AccountNo { get; set; }
        public string PFNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsEarningComponent { get; set; }
        public int SalaryHeadOrder { get; set; }
        public bool IsShowInSalarySlip { get; set; }
        public string SalaryHeadName { get; set; }

        public string PANNo { get; set; }
        public string UAN { get; set; }
        public string ESINo { get; set; }
        public string BranchName { get; set; }
        public string IFSC { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
