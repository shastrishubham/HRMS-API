using ServerModel.Model.Training;
using ServerModel.SqlAccess.EmployeeSetup.TrainingSetup;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Training
{
    public class TrainingInfoServer
    {
        #region Properties Interface

        public static ITrainingSetupAccess mTrainingSetupAccessT = new TrainingSetupAccessWrapper();

        #endregion

        public Guid UpsertEmployeeTraining(EmployeeTrainingInfo employeeTraining)
        {
            return mTrainingSetupAccessT.UpsertEmployeeTraining(employeeTraining);
        }

        public List<EmployeeTrainingInfo> GetEmployeeTrainingsByCompId(Guid compId)
        {
            return mTrainingSetupAccessT.GetEmployeeTrainingsByCompId(compId);
        }
    }
}
