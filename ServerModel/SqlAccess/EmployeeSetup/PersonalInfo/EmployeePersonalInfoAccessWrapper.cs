using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.PersonalInfo
{
    public class EmployeePersonalInfoAccessWrapper : IEmployeePersonalInfoAccess
    {
        public EmployeeInformation GetEmployeeApproverByEmpId(Guid employeeId)
        {
            return EmployeePersonalInfoAccess.GetEmployeeApproverByEmpId(employeeId);
        }
    }

    public class EmployeePersonalInfoAccessWrapper<T> : IEmployeePersonalInfoAccess<T> where T : EmployeePersonalInformation
    {
        public EmployeePersonalInformation GetEmployeeDetailByEmailAddress(string emailAddress)
        {
            return EmployeePersonalInfoAccess<T>.GetEmployeeDetailByEmailAddress(emailAddress);
        }

        public IEnumerable<EmployeePersonalInformation> GetEmployeePersonalInfoById(Guid employeeId)
        {
            return EmployeePersonalInfoAccess<T>.GetEmployeePersonalInfoById(employeeId);
        }

        public int GetNextEmpId(Guid compId)
        {
            return EmployeePersonalInfoAccess<T>.GetNextEmpId(compId);
        }

        public Guid UpsertEmployeePersonalInfo(EmployeePersonalInformation employeePersonalInformation)
        {
            return EmployeePersonalInfoAccess<T>.UpsertEmployeePersonalInfo(employeePersonalInformation);
        }
    }
}
