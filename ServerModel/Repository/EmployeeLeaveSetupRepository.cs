using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class EmployeeLeaveSetupRepository
    {
        private IRespository<EMP_Leaves> respository = null;

        public EmployeeLeaveSetupRepository()
        {
            this.respository = new Repository<EMP_Leaves>();
        }

        public DataResult AddUpdateEmployeeLeaveSetup(EmployeeLeaveSetupInformation employeeLeaveSetupInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                EMP_Leaves existingEmployeeLeaveSetupInfo = this.respository.GetById(employeeLeaveSetupInformation.Id);

                if (existingEmployeeLeaveSetupInfo == null)
                {
                    employeeLeaveSetupInformation.FormDate = DateTime.Now;
                    EMP_Leaves empLeaveSetupInfoDb = GetEmpLeaveSetupInfoDbFromEmployeeLeaveSetupInformation(employeeLeaveSetupInformation, Guid.NewGuid());
                    this.respository.Insert(empLeaveSetupInfoDb);
                }
                else
                {
                    existingEmployeeLeaveSetupInfo.MS_Leave_Id = employeeLeaveSetupInformation.MS_Leave_Id;
                    existingEmployeeLeaveSetupInfo.TotalLeaves = employeeLeaveSetupInformation.TotalLeaves;
                    existingEmployeeLeaveSetupInfo.AvailableLeaves = employeeLeaveSetupInformation.AvailableLeaves;

                    this.respository.Update(existingEmployeeLeaveSetupInfo);
                }
                this.respository.Save();

                dataResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                dataResult.ErrorMessage = ex.Message.ToString();
                dataResult.IsSuccess = false;
            }
            return dataResult;
        }

        public DataResult DeleteEmployeeLeaveSetup(EmployeeLeaveSetupInformation employeeLeaveSetupInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                if (employeeLeaveSetupInformation.Id != null)
                {
                    EMP_Leaves existingEmployeeLeaveSetupInfo = this.respository.GetById(employeeLeaveSetupInformation.Id);

                    if(existingEmployeeLeaveSetupInfo != null)
                    {
                        existingEmployeeLeaveSetupInfo.Active = false;

                        this.respository.Update(existingEmployeeLeaveSetupInfo);
                    }
                }

                this.respository.Save();

                dataResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                dataResult.ErrorMessage = ex.Message.ToString();
                dataResult.IsSuccess = false;
            }
            return dataResult;
        }

        #region DB Model to Server Model Data Binding

        private EMP_Leaves GetEmpLeaveSetupInfoDbFromEmployeeLeaveSetupInformation(EmployeeLeaveSetupInformation employeeLeaveSetupInformation, Guid existingEmployeeId)
        {
            EMP_Leaves empLeaveSetupInfoDb = new EMP_Leaves();
            empLeaveSetupInfoDb.Id = existingEmployeeId;
            empLeaveSetupInfoDb.FormDate = employeeLeaveSetupInformation.FormDate;
            empLeaveSetupInfoDb.MachineId = "AJINKYA-PC";
            empLeaveSetupInfoDb.MachineIp = "0.0.0.0";
            empLeaveSetupInfoDb.CompId = employeeLeaveSetupInformation.CompId;
            empLeaveSetupInfoDb.CreatedBy = employeeLeaveSetupInformation.CreatedBy;
            empLeaveSetupInfoDb.EMP_Info_Id = employeeLeaveSetupInformation.EMP_Info_Id;
            empLeaveSetupInfoDb.MS_Leave_Id = employeeLeaveSetupInformation.MS_Leave_Id;
            empLeaveSetupInfoDb.TotalLeaves = employeeLeaveSetupInformation.TotalLeaves;
            empLeaveSetupInfoDb.AvailableLeaves = employeeLeaveSetupInformation.AvailableLeaves;
            empLeaveSetupInfoDb.Active = true;

            return empLeaveSetupInfoDb;
        }

        #endregion

    }
}
