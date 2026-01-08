using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.QualificationInfo
{
    public class EmployeeQualificationInfoAccessWrapper
    {
    }

    public class EmployeeQualificationInfoAccessWrapper<T> : IEmployeeQualificationInfoAccess<T> where T : EmployeeQualificationInformation
    {
        public IEnumerable<EmployeeQualificationInformation> GetEmployeeQualificationInfoById(Guid employeeId)
        {
            return EmployeeQualificationInfoAccess<T>.GetEmployeeQualificationInfoById(employeeId);
        }

        public bool UpsertEmployeeQualificationInfo(List<EmployeeQualificationInformation> employeeQualificationInformations)
        {
            return EmployeeQualificationInfoAccess<T>.UpsertEmployeeQualificationInfo(employeeQualificationInformations);
        }
    }
}
