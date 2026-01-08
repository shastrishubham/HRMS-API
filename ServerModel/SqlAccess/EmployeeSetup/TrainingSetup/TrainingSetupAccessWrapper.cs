using ServerModel.Model.Training;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeSetup.TrainingSetup
{
    public class TrainingSetupAccessWrapper : ITrainingSetupAccess
    {
        public List<EmployeeTrainingInfo> GetEmployeeTrainingsByCompId(Guid compId)
        {
            return TrainingSetupAccess.GetEmployeeTrainingsByCompId(compId);
        }

        public Guid UpsertEmployeeTraining(EmployeeTrainingInfo employeeTraining)
        {
            return TrainingSetupAccess.UpsertEmployeeTraining(employeeTraining);
        }
    }
}
