using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeeSalarySetupDetails
    {
        public Guid Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineIp { get; set; }
        public string MachineId { get; set; }
        public Guid CompId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid EmpId { get; set; }
        public decimal TotalEarningAmt { get; set; }
        public decimal TotalDeductionAmt { get; set; }
        public int MS_PayMode_Id { get; set; }
        public List<EmployeeSalaryHeadsSetupDetails> employeeSalaryHeadsSetupDetails { get; set; }
        public bool Active { get; set; }

        public string FullName { get; set; }
        public string DesignationName { get; set; }
        public bool isDesignationWiseSalary { get; set; }
        public bool isForAllDesignationEmployee { get; set; }
        public int designationId { get; set; }
    }
}
