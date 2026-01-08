using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository.Recruitment
{
    public class JobVacancyFormRepository
    {
        private IRespository<Req_JbVacancy> respository = null;


        public JobVacancyFormRepository()
        {
            this.respository = new Repository<Req_JbVacancy>();
        }

        public DataResult AddUpdateCreateJobVacancy(JobVacancyForm jobVacancyForm)
        {
            DataResult dataResult = new DataResult();
            try
            {
                Req_JbVacancy existingJobVacancyInfo = this.respository.GetById(jobVacancyForm.Id);

                if (existingJobVacancyInfo == null)
                {
                    jobVacancyForm.FormDate = DateTime.Now;
                    Req_JbVacancy jobVacancyInfoDb = GetJobVacancyInfoDbFromJobVacancyForm(jobVacancyForm, Guid.NewGuid());
                    this.respository.Insert(jobVacancyInfoDb);
                }
                else
                {
                    existingJobVacancyInfo.MS_Designation_Id = jobVacancyForm.MS_Designation_Id;
                    existingJobVacancyInfo.MS_Branch_Id = jobVacancyForm.MS_Branch_Id;
                    existingJobVacancyInfo.HiringManager_Id = jobVacancyForm.HiringManager_Id;
                    existingJobVacancyInfo.NosOfPosition = jobVacancyForm.NosOfPosition;
                    existingJobVacancyInfo.JobDesc = jobVacancyForm.JobDesc;

                    this.respository.Update(existingJobVacancyInfo);
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

        public DataResult DeleteJobVacancy(JobVacancyForm jobVacancyForm)
        {
            DataResult dataResult = new DataResult();
            try
            {
                if (jobVacancyForm.Id != null)
                {
                    Req_JbVacancy existingjobVacancyForm = this.respository.GetById(jobVacancyForm.Id);

                    if (existingjobVacancyForm != null)
                    {
                        this.respository.RemoveEntity(existingjobVacancyForm);
                    }
                }
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
        private Req_JbVacancy GetJobVacancyInfoDbFromJobVacancyForm(JobVacancyForm jobVacancyForm, Guid existingEmployeeId)
        {
            Req_JbVacancy jobVacancyInfoDb = new Req_JbVacancy();
            jobVacancyInfoDb.Id = existingEmployeeId;
            jobVacancyInfoDb.FormDate = jobVacancyForm.FormDate;
            jobVacancyInfoDb.MachineId = "AJINKYA-PC";
            jobVacancyInfoDb.MachineIp = "0.0.0.0";
            jobVacancyInfoDb.CompId = jobVacancyForm.CompId;
            jobVacancyInfoDb.CreatedBy = jobVacancyForm.CreatedBy;
            jobVacancyInfoDb.MS_Designation_Id = jobVacancyForm.MS_Designation_Id;
            jobVacancyInfoDb.MS_Branch_Id = jobVacancyForm.MS_Branch_Id;
            jobVacancyInfoDb.HiringManager_Id = jobVacancyForm.HiringManager_Id;
            jobVacancyInfoDb.NosOfPosition = jobVacancyForm.NosOfPosition;
            jobVacancyInfoDb.JobDesc = jobVacancyForm.JobDesc;
            jobVacancyInfoDb.Active = true;

            return jobVacancyInfoDb;
        }
        #endregion
    }
}
