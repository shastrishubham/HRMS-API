using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSalarySetup
{
    public interface IEmpSalarySetupAccess
    {
    }

    public interface IEmpSalarySetupAccess<T> where T : EmployeeSalarySetupDetails
    {
        int AddUpdateEmployeeSalarySetup(EmployeeSalarySetupDetails employeeSalarySetupDetails);

        List<EmployeeSalarySetupDetails> GetSalarySetupByCompId(Guid compId);

        List<EmployeeSalaryHeadsSetupDetails> GetEmpSalarySetupDetails(Guid compId, Guid empId);

        List<EmployeeSalarySetupDetails> GetUnSetupSalaryEmployeesByDesignationId(Guid compId, int designationId);
    }
}
