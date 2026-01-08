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
    public class OnlineFormApplicationRepository
    {
        private IRespository<Req_JbForm> respository = null;


        public OnlineFormApplicationRepository()
        {
            this.respository = new Repository<Req_JbForm>();
        }

        public DataResult AddUpdateOnlineFormApplication(OnlineFormApplication onlineFormApplication)
        {
            DataResult dataResult = new DataResult();
            try
            {
                Req_JbForm existingOnlineFormApplicationInfo = this.respository.GetById(onlineFormApplication.Id);

                if (existingOnlineFormApplicationInfo == null)
                {
                    onlineFormApplication.FormDate = DateTime.Now;
                    Req_JbForm jobApplicationInfoDb = GetJobApplicationInfoDbFromJobApplicationForm(onlineFormApplication, Guid.NewGuid());
                    this.respository.Insert(jobApplicationInfoDb);
                }
                else
                {
                    existingOnlineFormApplicationInfo.EMP_Info_Id = onlineFormApplication.EMP_Info_Id;
                    existingOnlineFormApplicationInfo.FullName = onlineFormApplication.FullName;
                    existingOnlineFormApplicationInfo.Email = onlineFormApplication.Email;
                    existingOnlineFormApplicationInfo.ContactNo = onlineFormApplication.ContactNo;
                    existingOnlineFormApplicationInfo.YearOfExp = onlineFormApplication.YearOfExp;
                    existingOnlineFormApplicationInfo.MS_Branch_Id = onlineFormApplication.MS_Branch_Id;
                    existingOnlineFormApplicationInfo.Req_JbVacancy_Id = onlineFormApplication.Req_JbVacancy_Id;
                    existingOnlineFormApplicationInfo.Resume = onlineFormApplication.Resume;
                    existingOnlineFormApplicationInfo.Comments = onlineFormApplication.Comments;

                    this.respository.Update(existingOnlineFormApplicationInfo);
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

        public DataResult DeleteOnlineFormApplication(OnlineFormApplication onlineFormApplication)
        {
            DataResult dataResult = new DataResult();
            try
            {
                if (onlineFormApplication.Id != null)
                {
                    Req_JbForm existingjobVacancyForm = this.respository.GetById(onlineFormApplication.Id);

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
        private Req_JbForm GetJobApplicationInfoDbFromJobApplicationForm(OnlineFormApplication onlineFormApplication, Guid existingEmployeeId)
        {
            Req_JbForm jobApplicationInfoDb = new Req_JbForm();
            jobApplicationInfoDb.Id = existingEmployeeId;
            jobApplicationInfoDb.FormDate = onlineFormApplication.FormDate;
            jobApplicationInfoDb.MachineId = "AJINKYA-PC";
            jobApplicationInfoDb.MachineIp = "0.0.0.0";
            jobApplicationInfoDb.CompId = onlineFormApplication.CompId;
            jobApplicationInfoDb.CreatedBy = onlineFormApplication.CreatedBy;
            jobApplicationInfoDb.EMP_Info_Id = onlineFormApplication.EMP_Info_Id;
            jobApplicationInfoDb.FullName = onlineFormApplication.FullName;
            jobApplicationInfoDb.Email = onlineFormApplication.Email;
            jobApplicationInfoDb.ContactNo = onlineFormApplication.ContactNo;
            jobApplicationInfoDb.YearOfExp = onlineFormApplication.YearOfExp;
            jobApplicationInfoDb.MS_Branch_Id = onlineFormApplication.MS_Branch_Id;
            jobApplicationInfoDb.Req_JbVacancy_Id = onlineFormApplication.Req_JbVacancy_Id;
            jobApplicationInfoDb.Resume = onlineFormApplication.Resume;
            jobApplicationInfoDb.Comments = onlineFormApplication.Comments;
            jobApplicationInfoDb.Active = true;

            return jobApplicationInfoDb;
        }
        #endregion
    }
}
