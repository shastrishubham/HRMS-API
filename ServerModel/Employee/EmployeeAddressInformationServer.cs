using ServerModel.Model;
using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.AddressesInfo;
using System;
using System.Collections.Generic;

namespace ServerModel.Employee
{
    public class EmployeeAddressInformationServer
    {
        #region Properties Interface

        public static IEmployeeAddressSetupAccess  mEmpAddresslInfoAccessT
            = new EmployeeAddressSetupAccessWrapper();

        #endregion

        public static List<EmployeeAddresses> GetEmployeeAddressesById(Guid employeeId)
        {
            return mEmpAddresslInfoAccessT.GetEmployeeAddressesById(employeeId);
        }

        public static EmployeeAddresses GetEmployeeAddressByAddressId(Guid addressId)
        {
            return mEmpAddresslInfoAccessT.GetEmployeeAddressByAddressId(addressId);
        }

        public static DataResult UpsertEmployeeAddresses(List<EmployeeAddresses> employeeAddresses)
        {
            bool result = mEmpAddresslInfoAccessT.UpsertEmployeeAddresses(employeeAddresses);
            if (result)
                return new DataResult { IsSuccess = true };
            else
                return new DataResult { IsSuccess = false };
        }
    }
}
