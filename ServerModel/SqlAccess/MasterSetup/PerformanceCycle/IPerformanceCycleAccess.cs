using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.PerformanceCycle
{
    public interface IPerformanceCycleAccess
    {
        List<Model.Masters.PerformanceCycle> GetPerformanceCyclesByCompId(Guid compId);

        Model.Masters.PerformanceCycle GetPerformanceCycleById(int cycleId);

        int UpsertPerformanceCycles(Model.Masters.PerformanceCycle performanceCycle);
    }
}
