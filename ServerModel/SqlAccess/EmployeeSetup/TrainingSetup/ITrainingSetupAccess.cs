using ServerModel.Model.Masters;
using ServerModel.Model.Training;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.EmployeeSetup.TrainingSetup
{
    public interface ITrainingSetupAccess
    {
        Guid UpsertEmployeeTraining(EmployeeTrainingInfo employeeTraining);

        List<EmployeeTrainingInfo> GetEmployeeTrainingsByCompId(Guid compId);
    }
}
