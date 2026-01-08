using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.WorkExperienceInfo
{
    public interface IEmployeeWorkExpInfoAccess
    {
    }

    public interface IEmployeeWorkExpInfoAccess<T> where T : EmployeeWorkExperienceInformation
    {
        bool UpsertEmployeeWorkExpInfo(List<EmployeeWorkExperienceInformation> employeeWorkExperienceInformations);

        IEnumerable<EmployeeWorkExperienceInformation> GetEmployeeWorkExpInfoById(Guid employeeId);
    }
}
