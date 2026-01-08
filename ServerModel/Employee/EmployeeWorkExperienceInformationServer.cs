using ServerModel.Model;
using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.WorkExperienceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Employee
{
    public class EmployeeWorkExperienceInformationServer
    {
        #region Properties Interface

        public static IEmployeeWorkExpInfoAccess<EmployeeWorkExperienceInformation> mEmpWorkExpInfoAccessT
            = new EmployeeWorkExpInfoAccessWrapper<EmployeeWorkExperienceInformation>();

        #endregion


        public static DataResult UpsertEmployeeWorkExpInfo(List<EmployeeWorkExperienceInformation> employeeWorkExperienceInformations)
        {
            bool result = mEmpWorkExpInfoAccessT.UpsertEmployeeWorkExpInfo(employeeWorkExperienceInformations);
            if (result)
                return new DataResult { IsSuccess = true };
            else
                return new DataResult { IsSuccess = false };
        }

        public static IEnumerable<EmployeeWorkExperienceInformation> GetEmployeeWorkExpInfoById(Guid employeeId)
        {
            return mEmpWorkExpInfoAccessT.GetEmployeeWorkExpInfoById(employeeId);
        }
    }
}
