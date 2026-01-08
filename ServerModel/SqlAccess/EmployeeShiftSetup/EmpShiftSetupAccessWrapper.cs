using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeShiftSetup
{
    public class EmpShiftSetupAccessWrapper : IEmpShiftSetupAccess
    {
        public List<EmployeeShiftInformation> GetEmlployeeShiftByBranchAndShift(int branchId, int shiftId)
        {
            return EmpShiftSetupAccess.GetEmlployeeShiftByBranchAndShift(branchId, shiftId);
        }

        public List<EmployeeShiftInformation> GetEmployeeShiftDetailByBranchId(Guid compId, int branchId)
        {
            return EmpShiftSetupAccess.GetEmployeeShiftDetailByBranchId(compId, branchId);
        }
    }
}
