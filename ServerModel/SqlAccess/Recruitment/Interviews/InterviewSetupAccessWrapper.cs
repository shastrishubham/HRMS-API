using ServerModel.Model.Base;
using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.Recruitment.Interviews
{
    public class InterviewSetupAccessWrapper : IInterviewSetupAccess
    {
        public List<InterviewFeedback> GetInterviewFeedbacksByCompIdAndRating(Guid compId, int interviewRateId = 0)
        {
            return InterviewSetupAccess.GetInterviewFeedbacksByCompIdAndRating(compId, interviewRateId);
        }

        public List<InterviewPortalInformation> GetScheduleInterviewsByCompId(Guid compId, InterviewStatusTypes interviewStatus = InterviewStatusTypes.None)
        {
            return InterviewSetupAccess.GetScheduleInterviewsByCompId(compId, interviewStatus);
        }
    }
}
