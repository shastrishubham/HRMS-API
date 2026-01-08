using ServerModel.Model;
using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.FamilyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Employee
{
    public class EmployeeFamilyInformationServer
    {
        #region Properties Interface

        public static IEmployeeFamilyInfoAccess<EmployeeFamilyInformation> mEmpFamilyInfoAccessT
            = new EmployeeFamilyInfoAccessWrapper<EmployeeFamilyInformation>();

        #endregion


        public static DataResult UpsertEmployeeFamilyInfo(List<EmployeeFamilyInformation> employeeFamilyInformations)
        {
            bool result = mEmpFamilyInfoAccessT.UpsertEmployeeFamilyInfo(employeeFamilyInformations);
            if (result)
                return new DataResult { IsSuccess = true };
            else
                return new DataResult { IsSuccess = false };
        }

        public static IEnumerable<EmployeeFamilyInformation> GetEmployeeFamilyInfoById(Guid employeeId)
        {
            return mEmpFamilyInfoAccessT.GetEmployeeFamilyInfoById(employeeId);
        }
    }
}
