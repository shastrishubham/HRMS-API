using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model.Employee;
using ServerModel.Model.Masters;
using ServerModel.Repository;
using ServerModel.SqlAccess.EmployeeShiftSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Masters
{
    public class EmployeeShiftHelper
    {
        IRespository<EMP_Shift> shiftRespository = null;
        IRespository<MS_Shift> shiftMasterRespository = null;
        IRespository<EMP_Info> empInfoRespository = null;


        public EmployeeShiftHelper()
        {
            shiftRespository = new Repository<EMP_Shift>();
            shiftMasterRespository = new Repository<MS_Shift>();
            empInfoRespository = new Repository<EMP_Info>();

        }

        #region Properties Interface

        public static IEmpShiftSetupAccess mEmpShiftSetupAccessT
            = new EmpShiftSetupAccessWrapper();

        #endregion


        public EmployeeShiftInformation GetEmployeeShiftInformation(Guid companyId, Guid empId)
        {
            var result = (from empShift in shiftRespository.GetAll()
                          join shiftMS in shiftMasterRespository.GetAll() on empShift.MS_Shift_Id equals shiftMS.Id
                          join empInfo in empInfoRespository.GetAll() on empShift.EMP_Info_Id equals empInfo.Id
                          where empShift.CompId == companyId
                                 && empShift.EMP_Info_Id == empId
                                 && empShift.IsAmmend == false
                                 && empInfo.IsActive == true
                          select new EmployeeShiftInformation
                          {
                              Id = empShift.Id,
                              EMP_Info_Id = empShift.EMP_Info_Id,
                              EmployeeBranchId = empInfo.MS_Branch_Id,
                              EmployeeFirstName = empInfo.FirstName,
                              EmployeeMiddleName = empInfo.MiddleName,
                              EmployeeLastName = empInfo.LastName,
                              EmployeeFullName = empInfo.FullName,
                              MS_Shift_Id = empShift.MS_Shift_Id,
                              ShiftName = shiftMS.ShiftName,
                              ShiftStartTime = shiftMS.StartTime,
                              ShiftEndTime = shiftMS.EndTime,
                              StartFrom = empShift.StartFrom,
                              EndTo = empShift.EndTo,
                              IsPermanentShift = empShift.IsPermanentShift,
                              WeeklyOffId = LeaveHelper.ParseWeeklyOffDayIds(shiftMS.WeeklyOffDay) // -1 means no weekly off
                          }).FirstOrDefault();
            return result;
        }

        public List<EmployeeShiftInformation> GetEmlployeeShiftByBranchAndShift(int branchId, int shiftId)
        {
            return mEmpShiftSetupAccessT.GetEmlployeeShiftByBranchAndShift(branchId, shiftId);
        }

        public List<EmployeeShiftInformation> GetEmployeeShiftDetailByBranchId(Guid compId, int branchId)
        {
            return mEmpShiftSetupAccessT.GetEmployeeShiftDetailByBranchId(compId, branchId);
        }
    }
}
