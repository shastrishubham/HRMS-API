using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.PerformanceCycle
{
    public class PerformanceCycleAccessWraper : IPerformanceCycleAccess
    {
        public Model.Masters.PerformanceCycle GetPerformanceCycleById(int cycleId)
        {
            return PerformanceCycleAccess.GetPerformanceCycleById(cycleId);
        }

        public List<Model.Masters.PerformanceCycle> GetPerformanceCyclesByCompId(Guid compId)
        {
            return PerformanceCycleAccess.GetPerformanceCyclesByCompId(compId);
        }

        public int UpsertPerformanceCycles(Model.Masters.PerformanceCycle performanceCycle)
        {
            return PerformanceCycleAccess.UpsertPerformanceCycles(performanceCycle);
        }
    }
}
