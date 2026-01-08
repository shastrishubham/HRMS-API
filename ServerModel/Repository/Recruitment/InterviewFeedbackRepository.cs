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
    public class InterviewFeedbackRepository
    {
        private IRespository<Req_InterviewFedbck> respository = null;


        public InterviewFeedbackRepository()
        {
            this.respository = new Repository<Req_InterviewFedbck>();
        }

        public DataResult AddUpdateInterviewFeedback(InterviewFeedback interviewFeedback)
        {
            DataResult dataResult = new DataResult();
            try
            {
                Req_InterviewFedbck existingInterviewFeedbackInfo = this.respository.GetById(interviewFeedback.Id);

                if (existingInterviewFeedbackInfo == null)
                {
                    Req_InterviewFedbck interviewFeedbackInfoDb = GetInterviewFeedbackInfoDbFromInterviewFeedbackForm(interviewFeedback, Guid.NewGuid());
                    this.respository.Insert(interviewFeedbackInfoDb);
                }
                else
                {
                    existingInterviewFeedbackInfo.Req_InterviewSch_Id = interviewFeedback.Req_InterviewSch_Id;
                    existingInterviewFeedbackInfo.EMP_Info_Id = interviewFeedback.EMP_Info_Id;
                    existingInterviewFeedbackInfo.MS_InterviewRate_Id = interviewFeedback.MS_InterviewRate_Id;
                    existingInterviewFeedbackInfo.Feedback = interviewFeedback.Feedback;

                    this.respository.Update(existingInterviewFeedbackInfo);
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
        private Req_InterviewFedbck GetInterviewFeedbackInfoDbFromInterviewFeedbackForm(InterviewFeedback interviewFeedback, Guid existingEmployeeId)
        {
            Req_InterviewFedbck interviewFeedbackInfoDb = new Req_InterviewFedbck();
            interviewFeedbackInfoDb.Id = existingEmployeeId;
            interviewFeedbackInfoDb.FormDate = DateTime.Now;
            interviewFeedbackInfoDb.MachineId = "AJINKYA-PC";
            interviewFeedbackInfoDb.MachineIp = "0.0.0.0";
            interviewFeedbackInfoDb.CompId = interviewFeedback.CompId;
            interviewFeedbackInfoDb.CreatedBy = interviewFeedback.CreatedBy;
            interviewFeedbackInfoDb.Req_InterviewSch_Id = interviewFeedback.Req_InterviewSch_Id;
            interviewFeedbackInfoDb.EMP_Info_Id = interviewFeedback.EMP_Info_Id;
            interviewFeedbackInfoDb.MS_InterviewRate_Id = interviewFeedback.MS_InterviewRate_Id;
            interviewFeedbackInfoDb.Feedback = interviewFeedback.Feedback;
            return interviewFeedbackInfoDb;
        }
        #endregion
    }
}
