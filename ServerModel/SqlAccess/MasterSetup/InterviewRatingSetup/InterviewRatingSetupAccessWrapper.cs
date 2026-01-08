using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.InterviewRatingSetup
{
    public class InterviewRatingSetupAccessWrapper : IInterviewRatingSetupAccess
    {
        public List<InterviewRatingInfo> GetInterviewRating(Guid compId)
        {
            return InterviewRatingSetupAccess.GetInterviewRating(compId);
        }

        public int UpsertInterviewRating(InterviewRatingInfo interviewRating)
        {
            return InterviewRatingSetupAccess.UpsertInterviewRating(interviewRating);
        }
    }
}
