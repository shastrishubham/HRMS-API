using ServerModel.Model;
using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.QualificationInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Employee
{
    public class EmployeeQualificationInformationServer
    {
        #region Properties Interface

        public static IEmployeeQualificationInfoAccess<EmployeeQualificationInformation> mEmpQualificationInfoAccessT
            = new EmployeeQualificationInfoAccessWrapper<EmployeeQualificationInformation>();

        #endregion


        public static DataResult UpsertEmployeeQualificationInfo(List<EmployeeQualificationInformation> employeeQualificationInformations)
        {
           bool result = mEmpQualificationInfoAccessT.UpsertEmployeeQualificationInfo(employeeQualificationInformations);
            if (result)
                return new DataResult { IsSuccess = true };
            else
                return new DataResult { IsSuccess = false };
        }

        public static IEnumerable<EmployeeQualificationInformation> GetEmployeeQualificationInfoById(Guid employeeId)
        {
            return mEmpQualificationInfoAccessT.GetEmployeeQualificationInfoById(employeeId);
        }
    }
}
