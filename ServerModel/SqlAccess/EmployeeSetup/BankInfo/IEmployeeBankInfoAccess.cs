using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.BankInfo
{
    public interface IEmployeeBankInfoAccess
    {
    }

    public interface IEmployeeBankInfoAccess<T> where T : EmployeeBankInformation
    {
        int UpsertEmployeeBankInfo(EmployeeBankInformation employeeBankInformation);

        EmployeeBankInformation GetEmployeeBankInfoById(Guid employeeId, Guid compId);
    }
}
