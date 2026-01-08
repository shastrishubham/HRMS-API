using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.BankInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Employee
{
    public class EmployeeBankInformationServer
    {
        #region Properties Interface

        public static IEmployeeBankInfoAccess<EmployeeBankInformation> mEmpBankInfoAccessT
            = new EmployeeBankInfoAccessWrapper<EmployeeBankInformation>();

        #endregion

        public static int UpsertEmployeeBankInfo(EmployeeBankInformation employeeBankInformation)
        {
            return mEmpBankInfoAccessT.UpsertEmployeeBankInfo(employeeBankInformation);
        }

        public static EmployeeBankInformation GetEmployeeBankInfoById(Guid employeeId, Guid compId)
        {
            return mEmpBankInfoAccessT.GetEmployeeBankInfoById(employeeId, compId);
        }

    }
}
