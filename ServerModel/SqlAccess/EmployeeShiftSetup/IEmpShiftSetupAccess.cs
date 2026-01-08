using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeShiftSetup
{
    public interface IEmpShiftSetupAccess
    {
        List<EmployeeShiftInformation> GetEmlployeeShiftByBranchAndShift(int branchId, int shiftId);

        List<EmployeeShiftInformation> GetEmployeeShiftDetailByBranchId(Guid compId, int branchId);
    }
}
