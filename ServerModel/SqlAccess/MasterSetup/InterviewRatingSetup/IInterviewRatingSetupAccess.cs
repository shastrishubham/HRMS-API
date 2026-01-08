using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.InterviewRatingSetup
{
    public interface IInterviewRatingSetupAccess
    {
        List<InterviewRatingInfo> GetInterviewRating(Guid compId);

        int UpsertInterviewRating(InterviewRatingInfo interviewRating);
    }
}
