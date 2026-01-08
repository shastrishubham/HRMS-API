using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeSetup.AddressesInfo
{
    public class EmployeeAddressSetupAccessWrapper : IEmployeeAddressSetupAccess
    {
        public EmployeeAddresses GetEmployeeAddressByAddressId(Guid addressId)
        {
            return EmployeeAddressSetupAccess.GetEmployeeAddressByAddressId(addressId);
        }

        public List<EmployeeAddresses> GetEmployeeAddressesById(Guid employeeId)
        {
            return EmployeeAddressSetupAccess.GetEmployeeAddressesById(employeeId);
        }

        public bool UpsertEmployeeAddresses(List<EmployeeAddresses> empAddresses)
        {
            return EmployeeAddressSetupAccess.UpsertEmployeeAddresses(empAddresses);
        }
    }
}
