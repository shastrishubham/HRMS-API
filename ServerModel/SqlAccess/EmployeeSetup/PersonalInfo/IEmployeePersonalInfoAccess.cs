using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.PersonalInfo
{
    public interface IEmployeePersonalInfoAccess
    {
        EmployeeInformation GetEmployeeApproverByEmpId(Guid employeeId);
    }

    public interface IEmployeePersonalInfoAccess<T> where T : EmployeePersonalInformation
    {
        Guid UpsertEmployeePersonalInfo(EmployeePersonalInformation employeePersonalInformation);

        IEnumerable<EmployeePersonalInformation> GetEmployeePersonalInfoById(Guid employeeId);

        int GetNextEmpId(Guid compId);

        EmployeePersonalInformation GetEmployeeDetailByEmailAddress(string emailAddress);
    }
}
