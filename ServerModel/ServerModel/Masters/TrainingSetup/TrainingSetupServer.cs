using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.TrainingSetup;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Masters.TrainingSetup
{
    public class TrainingSetupServer
    {
        #region Properties Interface

        public static ITrainingSetup mTrainingSetupAccessT
            = new TrainingSetupAccessWrapper();

        #endregion

        public static List<TrainingInfo> GetTrainingsByCompId(Guid companyId)
        {
            return mTrainingSetupAccessT.GetTrainingsByCompId(companyId);
        }

        public static int UpsertTrainingSetup(TrainingInfo trainingInfo)
        {
            return mTrainingSetupAccessT.UpsertTrainingSetup(trainingInfo);
        }

        public static List<TrainingInfo> GetTrainingsByDesignationIdAndCompId(Guid compId, int designationId)
        {
            return mTrainingSetupAccessT.GetTrainingsByDesignationIdAndCompId(compId, designationId);
        }
    }
}
