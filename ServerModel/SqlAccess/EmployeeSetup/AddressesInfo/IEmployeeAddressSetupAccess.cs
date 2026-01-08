using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeSetup.AddressesInfo
{
    public interface IEmployeeAddressSetupAccess
    {
        bool UpsertEmployeeAddresses(List<EmployeeAddresses> empAddresses);

        List<EmployeeAddresses> GetEmployeeAddressesById(Guid employeeId);

        EmployeeAddresses GetEmployeeAddressByAddressId(Guid addressId);
    }
}
