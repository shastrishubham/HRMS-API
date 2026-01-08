using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.TrainingSetup
{
    public interface ITrainingSetup
    {
        int UpsertTrainingSetup(TrainingInfo trainingInfo);

        List<TrainingInfo> GetTrainingsByCompId(Guid compId);

        List<TrainingInfo> GetTrainingsByDesignationIdAndCompId(Guid compId, int designationId);
    }
}
