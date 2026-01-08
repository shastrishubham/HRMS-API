using ServerModel.Model.Base;
using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.Recruitment.Interviews
{
    public interface IInterviewSetupAccess
    {
        List<InterviewPortalInformation> GetScheduleInterviewsByCompId(Guid compId, InterviewStatusTypes interviewStatus = InterviewStatusTypes.None);

        List<InterviewFeedback> GetInterviewFeedbacksByCompIdAndRating(Guid compId, int interviewRateId = 0);
    }
}
