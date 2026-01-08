using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.WorkExperienceInfo
{
    public class EmployeeWorkExpInfoAccessWrapper
    {
    }

    public class EmployeeWorkExpInfoAccessWrapper<T> : IEmployeeWorkExpInfoAccess<T> where T : EmployeeWorkExperienceInformation
    {
        public IEnumerable<EmployeeWorkExperienceInformation> GetEmployeeWorkExpInfoById(Guid employeeId)
        {
            return EmployeeWorkExpInfoAccess<T>.GetEmployeeWorkExpInfoById(employeeId);
        }

        public bool UpsertEmployeeWorkExpInfo(List<EmployeeWorkExperienceInformation> employeeWorkExperienceInformations)
        {
            return EmployeeWorkExpInfoAccess<T>.UpsertEmployeeWorkExpInfo(employeeWorkExperienceInformations);
        }
    }
}
