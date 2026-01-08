using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class EmployeeShiftSetupRepository
    {
        private IRespository<EMP_Shift> respository = null;

        public EmployeeShiftSetupRepository()
        {
            this.respository = new Repository<EMP_Shift>();
        }

        public void AddUpdateEmployeeShiftInformation(EmployeeShiftInformation employeeShiftInformation)
        {
            EMP_Shift existingEmployeeShiftData = this.respository.GetAll().Where(x => x.EMP_Info_Id == employeeShiftInformation.EMP_Info_Id && x.IsAmmend == false).FirstOrDefault();

            if (existingEmployeeShiftData == null)
            {
                // insert
                EMP_Shift empShiftDb = GetEmpShiftInfoDbFromEmployeeShiftInformation(employeeShiftInformation, Guid.NewGuid());
                empShiftDb.IsAmmend = false;
                this.respository.Insert(empShiftDb);
            }
            else
            {
                // update
                existingEmployeeShiftData.IsAmmend = true;
                existingEmployeeShiftData.ModifiedBy = employeeShiftInformation.ModifiedBy;
                existingEmployeeShiftData.ModifiedOn = DateTime.UtcNow;
                this.respository.Update(existingEmployeeShiftData);

                EMP_Shift empShiftDb = GetEmpShiftInfoDbFromEmployeeShiftInformation(employeeShiftInformation, Guid.NewGuid());
                empShiftDb.IsAmmend = false;
                this.respository.Insert(empShiftDb);
            }
            this.respository.Save();
        }

        


        #region DB Model to Server Model Data Binding
        private EMP_Shift GetEmpShiftInfoDbFromEmployeeShiftInformation(EmployeeShiftInformation employeeShiftInformation, Guid existingEmployeeId)
        {
            EMP_Shift empShiftDb = new EMP_Shift();
            empShiftDb.Id = existingEmployeeId;
            empShiftDb.FormDate = DateTime.Now;
            empShiftDb.MachineId = "AJINKYA-PC";
            empShiftDb.MachineIp = "0.0.0.0";
            empShiftDb.CompId = employeeShiftInformation.CompId;
            empShiftDb.CreatedBy = employeeShiftInformation.CreatedBy;
            empShiftDb.CreatedOn = DateTime.UtcNow;
            empShiftDb.EMP_Info_Id = employeeShiftInformation.EMP_Info_Id;
            empShiftDb.MS_Shift_Id = employeeShiftInformation.MS_Shift_Id;
            empShiftDb.StartFrom = employeeShiftInformation.StartFrom;
            empShiftDb.EndTo = employeeShiftInformation.EndTo;
            empShiftDb.IsPermanentShift = employeeShiftInformation.IsPermanentShift;
            empShiftDb.ModifiedBy = employeeShiftInformation.ModifiedBy;
            empShiftDb.ModifiedOn = DateTime.UtcNow;
            return empShiftDb;
        }

        private EmployeeShiftInformation GetEmployeeShiftInformationFromEmpInfoDb(EMP_Shift empShiftDb)
        {
            EmployeeShiftInformation employeeShiftInformation = new EmployeeShiftInformation();
            employeeShiftInformation.Id = empShiftDb.Id;
            employeeShiftInformation.FormDate = empShiftDb.FormDate;
            employeeShiftInformation.MachineId = empShiftDb.MachineId;
            employeeShiftInformation.MachineIp = empShiftDb.MachineIp;
            employeeShiftInformation.CompId = empShiftDb.CompId;
            employeeShiftInformation.CreatedBy = empShiftDb.CreatedBy;
            employeeShiftInformation.CreatedOn = empShiftDb.CreatedOn;
            employeeShiftInformation.EMP_Info_Id = empShiftDb.EMP_Info_Id;
            employeeShiftInformation.MS_Shift_Id = empShiftDb.MS_Shift_Id;
            employeeShiftInformation.StartFrom = empShiftDb.StartFrom;
            employeeShiftInformation.EndTo = empShiftDb.EndTo;
            employeeShiftInformation.IsPermanentShift = empShiftDb.IsPermanentShift;
            employeeShiftInformation.ModifiedBy = empShiftDb.ModifiedBy;
            employeeShiftInformation.ModifiedOn = empShiftDb.ModifiedOn;
            return employeeShiftInformation;
        }
        #endregion

    }
}
