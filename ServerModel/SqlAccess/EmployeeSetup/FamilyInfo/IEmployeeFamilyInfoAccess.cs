using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.FamilyInfo
{
    public interface IEmployeeFamilyInfoAccess
    {
    }

    public interface IEmployeeFamilyInfoAccess<T> where T : EmployeeFamilyInformation
    {
        bool UpsertEmployeeFamilyInfo(List<EmployeeFamilyInformation> employeeFamilyInformations);

        IEnumerable<EmployeeFamilyInformation> GetEmployeeFamilyInfoById(Guid employeeId);
    }
}
