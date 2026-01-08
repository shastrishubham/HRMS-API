using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSalarySetup
{
    public class EmpSalarySetupAccessWrapper : IEmpSalarySetupAccess
    {
    }

    public class EmpSalarySetupAccessWrapper<T> : IEmpSalarySetupAccess<T> where T : EmployeeSalarySetupDetails
    {
        public int AddUpdateEmployeeSalarySetup(EmployeeSalarySetupDetails employeeSalarySetupDetails)
        {
            return EmpSalarySetupAccess<T>.AddUpdateEmployeeSalarySetup(employeeSalarySetupDetails);
        }

        public List<EmployeeSalaryHeadsSetupDetails> GetEmpSalarySetupDetails(Guid compId, Guid empId)
        {
            return EmpSalarySetupAccess<T>.GetEmpSalarySetupDetails(compId, empId);
        }

        public List<EmployeeSalarySetupDetails> GetSalarySetupByCompId(Guid compId)
        {
            return EmpSalarySetupAccess<T>.GetSalarySetupByCompId(compId);
        }

        public List<EmployeeSalarySetupDetails> GetUnSetupSalaryEmployeesByDesignationId(Guid compId, int designationId)
        {
            return EmpSalarySetupAccess<T>.GetUnSetupSalaryEmployeesByDesignationId(compId, designationId);
        }
    }
}
