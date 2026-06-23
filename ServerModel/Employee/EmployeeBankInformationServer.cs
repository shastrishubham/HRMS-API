using ServerModel.Model;
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

        public static DataResult UpsertEmployeeBankInfo(EmployeeBankInformation employeeBankInformation)
        {
            int id = mEmpBankInfoAccessT.UpsertEmployeeBankInfo(employeeBankInformation);
            if (id > 0)
            {
                return new DataResult
                {
                    IsSuccess = true,
                    data = GetEmployeeBankInfoById(employeeBankInformation.EMP_Info_Id, employeeBankInformation.CompId)
                };
            }

            return new DataResult { IsSuccess = false };
        }

        public static EmployeeBankInformation GetEmployeeBankInfoById(Guid employeeId, Guid compId)
        {
            return mEmpBankInfoAccessT.GetEmployeeBankInfoById(employeeId, compId);
        }

    }
}
