using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.QualificationInfo
{
    public interface IEmployeeQualificationInfoAccess
    {
    }

    public interface IEmployeeQualificationInfoAccess<T> where T : EmployeeQualificationInformation
    {
        bool UpsertEmployeeQualificationInfo(List<EmployeeQualificationInformation> employeeQualificationInformations);

        IEnumerable<EmployeeQualificationInformation> GetEmployeeQualificationInfoById(Guid employeeId);
    }
}
