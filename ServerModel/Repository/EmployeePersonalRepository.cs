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
    public class EmployeePersonalRepository
    {
        private IRespository<EMP_PersonalInfo> respository = null;

        public EmployeePersonalRepository()
        {
            this.respository = new Repository<EMP_PersonalInfo>();
        }

        public DataResult AddUpdateEmployeePersonalInformation(EmployeePersonalInformation employeePersonalInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                EMP_PersonalInfo existingEmployeePersonalInfo = this.respository.GetById(employeePersonalInformation.Id);

                if (existingEmployeePersonalInfo == null)
                {
                    employeePersonalInformation.FormDate = DateTime.Now;
                    EMP_PersonalInfo empInfoDb = GetEmpPersonalInfoDbFromEmployeePersonalInformation(employeePersonalInformation, Guid.NewGuid());
                    this.respository.Insert(empInfoDb);
                }
                else
                {
                    existingEmployeePersonalInfo.Email = employeePersonalInformation.Email;
                    existingEmployeePersonalInfo.Mobile1 = employeePersonalInformation.Mobile1;
                    existingEmployeePersonalInfo.Mobile2 = employeePersonalInformation.Mobile2;
                    existingEmployeePersonalInfo.MaritialStatus = employeePersonalInformation.MaritialStatus;
                    existingEmployeePersonalInfo.MarrigeDate = employeePersonalInformation.MarrigeDate;
                    existingEmployeePersonalInfo.NoOfChildren = employeePersonalInformation.NoOfChildren;
                    existingEmployeePersonalInfo.BirthPlace = employeePersonalInformation.BirthPlace;
                    existingEmployeePersonalInfo.Religion = employeePersonalInformation.Religion;
                    existingEmployeePersonalInfo.LICNo = employeePersonalInformation.LICNo;
                    existingEmployeePersonalInfo.PassportNo = employeePersonalInformation.PassportNo;
                    existingEmployeePersonalInfo.VehicleNo = employeePersonalInformation.VehicleNo;
                    existingEmployeePersonalInfo.BloodGroup = employeePersonalInformation.BloodGroup;
                    existingEmployeePersonalInfo.Hobbies = employeePersonalInformation.Hobbies;
                    this.respository.Update(existingEmployeePersonalInfo);
                }
                this.respository.Save();

                dataResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                dataResult.IsSuccess = false;
                dataResult.ErrorMessage = ex.Message.ToString();
            }
            return dataResult;
        }

        public IEnumerable<EMP_PersonalInfo> GetAll()
        {
            IEnumerable<EMP_PersonalInfo> empPersonalInfoDb = this.respository.GetAll();
            return empPersonalInfoDb;
        }

        #region DB Model to Server Model Data Binding
        private EMP_PersonalInfo GetEmpPersonalInfoDbFromEmployeePersonalInformation(EmployeePersonalInformation employeePersonalInformation, Guid existingEmployeeId)
        {
            EMP_PersonalInfo empPersonalInfoDb = new EMP_PersonalInfo();
            empPersonalInfoDb.Id = existingEmployeeId;
            empPersonalInfoDb.FormDate = employeePersonalInformation.FormDate;
            empPersonalInfoDb.MachineId = "AJINKYA-PC";
            empPersonalInfoDb.MachineIp = "0.0.0.0";
            empPersonalInfoDb.CompId = employeePersonalInformation.CompId;
            empPersonalInfoDb.CreatedBy = employeePersonalInformation.CreatedBy;
            empPersonalInfoDb.EMP_Info_Id = employeePersonalInformation.EMP_Info_Id;
            empPersonalInfoDb.Email = employeePersonalInformation.Email;
            empPersonalInfoDb.Mobile1 = employeePersonalInformation.Mobile1;
            empPersonalInfoDb.Mobile2 = employeePersonalInformation.Mobile2;
            empPersonalInfoDb.MaritialStatus = employeePersonalInformation.MaritialStatus;
            empPersonalInfoDb.MarrigeDate = employeePersonalInformation.MarrigeDate;
            empPersonalInfoDb.NoOfChildren = employeePersonalInformation.NoOfChildren;
            empPersonalInfoDb.BirthPlace = employeePersonalInformation.BirthPlace;
            empPersonalInfoDb.Religion = employeePersonalInformation.Religion;
            empPersonalInfoDb.LICNo = employeePersonalInformation.LICNo;
            empPersonalInfoDb.PassportNo = employeePersonalInformation.PassportNo;
            empPersonalInfoDb.VehicleNo = employeePersonalInformation.VehicleNo;
            empPersonalInfoDb.BloodGroup = employeePersonalInformation.BloodGroup;
            empPersonalInfoDb.Hobbies = employeePersonalInformation.Hobbies;
            return empPersonalInfoDb;
        }
        
        #endregion
    }
}
