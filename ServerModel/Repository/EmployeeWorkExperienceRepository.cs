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
    public class EmployeeWorkExperienceRepository
    {
        private IRespository<EMP_WorkExp> respository = null;

        public EmployeeWorkExperienceRepository()
        {
            this.respository = new Repository<EMP_WorkExp>();
        }

        public DataResult AddUpdateEmployeeWorkExperienceInfo(EmployeeWorkExperienceInformation employeeWorkExperienceInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                EMP_WorkExp existingEmployeeWorkExperienceInfo = this.respository.GetById(employeeWorkExperienceInformation.Id);

                if (existingEmployeeWorkExperienceInfo == null)
                {
                    employeeWorkExperienceInformation.FormDate = DateTime.Now;
                    EMP_WorkExp empQualiInfoDb = GetEmpWorkExperienceInfoDbFromEmployeeWorkExperienceInformation(employeeWorkExperienceInformation, Guid.NewGuid());
                    this.respository.Insert(empQualiInfoDb);
                }
                else
                {
                    existingEmployeeWorkExperienceInfo.PreviousEmployer = employeeWorkExperienceInformation.PreviousEmployer;
                    existingEmployeeWorkExperienceInfo.EmployerAddress = employeeWorkExperienceInformation.EmployerAddress;
                    existingEmployeeWorkExperienceInfo.FromDate = employeeWorkExperienceInformation.FromDate;
                    existingEmployeeWorkExperienceInfo.ToDate = employeeWorkExperienceInformation.ToDate;
                    existingEmployeeWorkExperienceInfo.BasicSalary = employeeWorkExperienceInformation.BasicSalary;
                    existingEmployeeWorkExperienceInfo.NetSalary = employeeWorkExperienceInformation.NetSalary;
                    existingEmployeeWorkExperienceInfo.Designation = employeeWorkExperienceInformation.Designation;
                    existingEmployeeWorkExperienceInfo.ReportingSupervisorName = employeeWorkExperienceInformation.ReportingSupervisorName;
                    existingEmployeeWorkExperienceInfo.SupervisorNo = employeeWorkExperienceInformation.SupervisorNo;
                    existingEmployeeWorkExperienceInfo.LeavingReason = employeeWorkExperienceInformation.LeavingReason;

                    this.respository.Update(existingEmployeeWorkExperienceInfo);
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

        public IEnumerable<EmployeeWorkExperienceInformation> GetEmployeeWorkExperiencesByEmployeeId(Guid employeeId)
        {
            var empWorkExps = from empWorkExperience in this.respository.GetAll()
                                   where empWorkExperience.EMP_Info_Id == employeeId
                                   select new EmployeeWorkExperienceInformation
                                   {
                                      Id = empWorkExperience.Id,
                                      EMP_Info_Id = empWorkExperience.EMP_Info_Id,
                                      PreviousEmployer = empWorkExperience.PreviousEmployer,
                                      EmployerAddress = empWorkExperience.EmployerAddress,
                                      FromDate = empWorkExperience.FromDate.Value.Date,
                                      ToDate = empWorkExperience.ToDate,
                                      BasicSalary = empWorkExperience.BasicSalary,
                                      NetSalary = empWorkExperience.NetSalary,
                                      Designation = empWorkExperience.Designation,
                                      ReportingSupervisorName = empWorkExperience.ReportingSupervisorName,
                                      SupervisorNo = empWorkExperience.SupervisorNo,
                                      LeavingReason = empWorkExperience.LeavingReason
                                   };
            return empWorkExps;
        }


        #region DB Model to Server Model Data Binding

        private EMP_WorkExp GetEmpWorkExperienceInfoDbFromEmployeeWorkExperienceInformation(EmployeeWorkExperienceInformation employeeWorkExperienceInformation, Guid existingEmployeeId)
        {
            EMP_WorkExp empWorkExperienceInfoDb = new EMP_WorkExp();
            empWorkExperienceInfoDb.Id = existingEmployeeId;
            empWorkExperienceInfoDb.FormDate = employeeWorkExperienceInformation.FormDate;
            empWorkExperienceInfoDb.MachineId = "AJINKYA-PC";
            empWorkExperienceInfoDb.MachineIp = "0.0.0.0";
            empWorkExperienceInfoDb.CompId = employeeWorkExperienceInformation.CompId;
            empWorkExperienceInfoDb.CreatedBy = employeeWorkExperienceInformation.CreatedBy;
            empWorkExperienceInfoDb.EMP_Info_Id = employeeWorkExperienceInformation.EMP_Info_Id;
            empWorkExperienceInfoDb.PreviousEmployer = employeeWorkExperienceInformation.PreviousEmployer;
            empWorkExperienceInfoDb.EmployerAddress = employeeWorkExperienceInformation.EmployerAddress;
            empWorkExperienceInfoDb.FromDate = employeeWorkExperienceInformation.FromDate;
            empWorkExperienceInfoDb.ToDate = employeeWorkExperienceInformation.ToDate;
            empWorkExperienceInfoDb.BasicSalary = employeeWorkExperienceInformation.BasicSalary;
            empWorkExperienceInfoDb.NetSalary = employeeWorkExperienceInformation.NetSalary;
            empWorkExperienceInfoDb.Designation = employeeWorkExperienceInformation.Designation;
            empWorkExperienceInfoDb.ReportingSupervisorName = employeeWorkExperienceInformation.ReportingSupervisorName;
            empWorkExperienceInfoDb.SupervisorNo = employeeWorkExperienceInformation.SupervisorNo;
            empWorkExperienceInfoDb.LeavingReason = employeeWorkExperienceInformation.LeavingReason;

            return empWorkExperienceInfoDb;
        }

        #endregion
    }
}
