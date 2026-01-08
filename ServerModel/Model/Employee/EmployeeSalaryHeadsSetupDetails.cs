using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeeSalaryHeadsSetupDetails
    {
        public Guid Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineIp { get; set; }
        public string MachineId { get; set; }
        public Guid CompId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid EmpId { get; set; }
        public int MS_SLHeads_Id { get; set; }
        public decimal Amount { get; set; }
        public bool Active { get; set; }

        public string SalaryHeadName { get; set; }
        public string ShortSalaryHeadName { get; set; }
        public bool IsEarningComponent { get; set; }
        public bool IsFixedComponent { get; set; }
        public bool IsTaxableComponent { get; set; }
        public int MS_PayMode_Id { get; set; }
        public int SalaryHeadOrder { get; set; }

        public Guid EMP_SLSetup_Id { get; set; }
    }
}
