using System;
using System.Collections.Generic;
using ServerModel.Model.Masters;

namespace ServerModel.SqlAccess.MasterSetup.TrainingSetup
{
    public class TrainingSetupAccessWrapper : ITrainingSetup
    {
        public List<TrainingInfo> GetTrainingsByCompId(Guid compId)
        {
            return TrainingSetupAccess.GetTrainingsByCompId(compId);
        }

        public List<TrainingInfo> GetTrainingsByDesignationIdAndCompId(Guid compId, int designationId)
        {
            return TrainingSetupAccess.GetTrainingsByDesignationIdAndCompId(compId, designationId);
        }

        public int UpsertTrainingSetup(TrainingInfo trainingInfo)
        {
            return TrainingSetupAccess.UpsertTrainingSetup(trainingInfo);
        }
    }
}
