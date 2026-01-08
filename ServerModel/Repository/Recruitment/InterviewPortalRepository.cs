using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Base;
using ServerModel.Model.Recruitment;
using ServerModel.SqlAccess.Recruitment.Interviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository.Recruitment
{
    public class InterviewPortalRepository
    {
        private IRespository<Req_InterviewSch> respository = null;


        public InterviewPortalRepository()
        {
            this.respository = new Repository<Req_InterviewSch>();
        }
        
       

        public DataResult AddUpdateInterviewScheduleApplication(InterviewPortalInformation interviewPortalInformation)
        {
            DataResult dataResult = new DataResult();
            try
            {
                Req_InterviewSch existingInterviewScheduleApplicationInfo = this.respository.GetById(interviewPortalInformation.Id);

                if (existingInterviewScheduleApplicationInfo == null)
                {
                    interviewPortalInformation.FormDate = DateTime.Now;
                    Req_InterviewSch interviewScheduleInfoDb = GetInterviewScheduleApplicationInfoDbFromInterviewScheduleForm(interviewPortalInformation, Guid.NewGuid());
                    this.respository.Insert(interviewScheduleInfoDb);
                }
                else
                {
                    existingInterviewScheduleApplicationInfo.Req_JbVacancy_Id = interviewPortalInformation.Req_JbVacancy_Id;
                    existingInterviewScheduleApplicationInfo.Req_JbForm_Id = interviewPortalInformation.Req_JbForm_Id;
                    existingInterviewScheduleApplicationInfo.InterviewDateTime = interviewPortalInformation.InterviewDateTime;
                    existingInterviewScheduleApplicationInfo.Method = interviewPortalInformation.Method;
                    existingInterviewScheduleApplicationInfo.EMP_Info_Id = interviewPortalInformation.EMP_Info_Id;
                    existingInterviewScheduleApplicationInfo.InterviewStatus = interviewPortalInformation.InterviewStatus;
                    existingInterviewScheduleApplicationInfo.Comments = interviewPortalInformation.Comments;

                    this.respository.Update(existingInterviewScheduleApplicationInfo);
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
        private Req_InterviewSch GetInterviewScheduleApplicationInfoDbFromInterviewScheduleForm(InterviewPortalInformation interviewPortalInformation, Guid existingEmployeeId)
        {
            Req_InterviewSch interviewScheduleInfoDb = new Req_InterviewSch();
            interviewScheduleInfoDb.Id = existingEmployeeId;
            interviewScheduleInfoDb.FormDate = interviewPortalInformation.FormDate;
            interviewScheduleInfoDb.MachineId = "AJINKYA-PC";
            interviewScheduleInfoDb.MachineIp = "0.0.0.0";
            interviewScheduleInfoDb.CompId = interviewPortalInformation.CompId;
            interviewScheduleInfoDb.CreatedBy = interviewPortalInformation.CreatedBy;
            interviewScheduleInfoDb.Req_JbVacancy_Id = interviewPortalInformation.Req_JbVacancy_Id;
            interviewScheduleInfoDb.Req_JbForm_Id = interviewPortalInformation.Req_JbForm_Id;
            interviewScheduleInfoDb.InterviewDateTime = interviewPortalInformation.InterviewDateTime;
            interviewScheduleInfoDb.Method = interviewPortalInformation.Method;
            interviewScheduleInfoDb.EMP_Info_Id = interviewPortalInformation.EMP_Info_Id;
            interviewScheduleInfoDb.InterviewStatus = interviewPortalInformation.InterviewStatus;
            interviewScheduleInfoDb.Comments = interviewPortalInformation.Comments;
            interviewScheduleInfoDb.Active = true;

            return interviewScheduleInfoDb;
        }
        #endregion
    }
}
