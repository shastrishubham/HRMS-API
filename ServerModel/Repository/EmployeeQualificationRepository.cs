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
    public class EmployeeQualificationRepository
    {
        private IRespository<EMP_Qualification> respository = null;

        public EmployeeQualificationRepository()
        {
            this.respository = new Repository<EMP_Qualification>();
        }

        public DataResult AddUpdateEmployeeQualificationInfo(EmployeeQualificationInformation employeeQualificationInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                EMP_Qualification existingEmployeeQualificationInfo = this.respository.GetById(employeeQualificationInformation.Id);

                if (existingEmployeeQualificationInfo == null)
                {
                    employeeQualificationInformation.FormDate = DateTime.Now;
                    EMP_Qualification empQualiInfoDb = GetEmpQualificationInfoDbFromEmployeeQualificationInformation(employeeQualificationInformation, Guid.NewGuid());
                    this.respository.Insert(empQualiInfoDb);
                }
                else
                {
                    existingEmployeeQualificationInfo.HighestQualification = employeeQualificationInformation.HighestQualification;
                    existingEmployeeQualificationInfo.UniversityName = employeeQualificationInformation.UniversityName;
                    existingEmployeeQualificationInfo.CollegeName = employeeQualificationInformation.CollegeName;
                    existingEmployeeQualificationInfo.MainSubjects = employeeQualificationInformation.MainSubjects;
                    existingEmployeeQualificationInfo.Division = employeeQualificationInformation.Division;
                    existingEmployeeQualificationInfo.PassingYear = employeeQualificationInformation.PassingYear;
                    existingEmployeeQualificationInfo.Percentage = employeeQualificationInformation.Percentage;
                   
                    this.respository.Update(existingEmployeeQualificationInfo);
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

        public IEnumerable<EmployeeQualificationInformation> GetEmployeeQualificationsByEmployeeId(Guid employeeId)
        {
            var empQulifications = from empQualification in this.respository.GetAll()
                                   where empQualification.EMP_Info_Id == employeeId
                                   select new EmployeeQualificationInformation
                                   {
                                       Id = empQualification.Id,
                                       EMP_Info_Id = empQualification.EMP_Info_Id,
                                       HighestQualification = empQualification.HighestQualification,
                                       UniversityName = empQualification.UniversityName,
                                       CollegeName = empQualification.CollegeName,
                                       MainSubjects = empQualification.MainSubjects,
                                       Division = empQualification.Division,
                                       PassingYear = empQualification.PassingYear,
                                       Percentage = empQualification.Percentage
                                   };
            return empQulifications;
        }

        #region DB Model to Server Model Data Binding

        private EMP_Qualification GetEmpQualificationInfoDbFromEmployeeQualificationInformation(EmployeeQualificationInformation employeeQualificationInformation, Guid existingEmployeeId)
        {
            EMP_Qualification empQualificationInfoDb = new EMP_Qualification();
            empQualificationInfoDb.Id = existingEmployeeId;
            empQualificationInfoDb.FormDate = employeeQualificationInformation.FormDate;
            empQualificationInfoDb.MachineId = "AJINKYA-PC";
            empQualificationInfoDb.MachineIp = "0.0.0.0";
            empQualificationInfoDb.CompId = employeeQualificationInformation.CompId;
            empQualificationInfoDb.CreatedBy = employeeQualificationInformation.CreatedBy;
            empQualificationInfoDb.EMP_Info_Id = employeeQualificationInformation.EMP_Info_Id;
            empQualificationInfoDb.HighestQualification = employeeQualificationInformation.HighestQualification;
            empQualificationInfoDb.UniversityName = employeeQualificationInformation.UniversityName;
            empQualificationInfoDb.CollegeName = employeeQualificationInformation.CollegeName;
            empQualificationInfoDb.MainSubjects = employeeQualificationInformation.MainSubjects;
            empQualificationInfoDb.Division = employeeQualificationInformation.Division;
            empQualificationInfoDb.PassingYear = employeeQualificationInformation.PassingYear;
            empQualificationInfoDb.Percentage = employeeQualificationInformation.Percentage;
            
            return empQualificationInfoDb;
        }

        #endregion
    }
}
