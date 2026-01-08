using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.InterviewRatingSetup;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Masters.InterviewRatingSetup
{
    public class InterviewRatingSetupServer
    {
        #region Properties Interface

        public static IInterviewRatingSetupAccess mInterviewRatingSetupAccessT
            = new InterviewRatingSetupAccessWrapper();

        #endregion


        public static List<InterviewRatingInfo> GetInterviewRating(Guid companyId)
        {
            return mInterviewRatingSetupAccessT.GetInterviewRating(companyId);
        }

        public static int UpsertInterviewRating(InterviewRatingInfo interviewRatingInfo)
        {
            return mInterviewRatingSetupAccessT.UpsertInterviewRating(interviewRatingInfo);
        }
    }
}
