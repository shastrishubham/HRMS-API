using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.BankInfo
{
    public class EmployeeBankInfoAccessWrapper : IEmployeeBankInfoAccess
    {
    }

    public class EmployeeBankInfoAccessWrapper<T> : IEmployeeBankInfoAccess<T> where T : EmployeeBankInformation
    {
        public EmployeeBankInformation GetEmployeeBankInfoById(Guid employeeId, Guid compId)
        {
            return EmployeeBankInfoAccess<T>.GetEmployeeBankInfoById(employeeId, compId);
        }

        public int UpsertEmployeeBankInfo(EmployeeBankInformation employeeBankInformation)
        {
            return EmployeeBankInfoAccess<T>.UpsertEmployeeBankInfo(employeeBankInformation);
        }

       
    }
}
