using ServerModel.Model;
using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.PersonalInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Employee
{
    public class EmployeePersonalInformationServer : EmployeePersonalInformation
    {
        #region Properties Interface

        public static IEmployeePersonalInfoAccess<EmployeePersonalInformation> mEmpPersonalInfoAccessT
            = new EmployeePersonalInfoAccessWrapper<EmployeePersonalInformation>();

        #endregion

        public static EmployeePersonalInformation GetEmployeeDetailByEmailAddress(string emailAddress)
        {
            return mEmpPersonalInfoAccessT.GetEmployeeDetailByEmailAddress(emailAddress);
        }

        public static DataResult UpsertEmployeePersonalInfo(EmployeePersonalInformation employeePersonalInformation)
        {
            Guid result = mEmpPersonalInfoAccessT.UpsertEmployeePersonalInfo(employeePersonalInformation);
            if (result != Guid.Empty)
                return new DataResult { IsSuccess = true };
            else
                return new DataResult { IsSuccess = false };
        }

        public static IEnumerable<EmployeePersonalInformation> GetEmployeePersonalInfoById(Guid employeeId)
        {
            return mEmpPersonalInfoAccessT.GetEmployeePersonalInfoById(employeeId);
        }

        public static int GetNextEmpId(Guid compId)
        {
            return mEmpPersonalInfoAccessT.GetNextEmpId(compId);
        }
    }
}
