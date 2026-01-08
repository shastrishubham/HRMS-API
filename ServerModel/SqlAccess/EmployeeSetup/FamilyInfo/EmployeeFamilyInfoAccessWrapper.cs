using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.FamilyInfo
{
    public class EmployeeFamilyInfoAccessWrapper : IEmployeeFamilyInfoAccess
    {
    }

    public class EmployeeFamilyInfoAccessWrapper<T> : IEmployeeFamilyInfoAccess<T> where T : EmployeeFamilyInformation
    {
        public IEnumerable<EmployeeFamilyInformation> GetEmployeeFamilyInfoById(Guid employeeId)
        {
            return EmployeeFamilyInfoAccess<T>.GetEmployeeFamilyInfoById(employeeId);
        }

        public bool UpsertEmployeeFamilyInfo(List<EmployeeFamilyInformation> employeeFamilyInformations)
        {
            return EmployeeFamilyInfoAccess<T>.UpsertEmployeeFamilyInfo(employeeFamilyInformations);
        }
    }
}
