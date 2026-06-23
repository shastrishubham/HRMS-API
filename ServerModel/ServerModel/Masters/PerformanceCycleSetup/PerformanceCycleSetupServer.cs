using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.PerformanceCycle;
using ServerModel.SqlAccess.MasterSetup.PMSGoalCategories;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Masters.PerformanceCycleSetup
{
    public class PerformanceCycleSetupServer
    {

        #region Properties Interface

        public static IPerformanceCycleAccess mPerformanceCycleAccess
            = new PerformanceCycleAccessWraper();

        public static IPMSGoalCategoryAccess mPMSGoalCategoryAccess
            = new PMSGoalCategoryAccessWrapper();

        #endregion


        #region Performance Cycle

        public static Model.Masters.PerformanceCycle GetPerformanceCycleById(int cycleId)
        {
           return mPerformanceCycleAccess.GetPerformanceCycleById(cycleId);
        }

        public static List<Model.Masters.PerformanceCycle> GetPerformanceCyclesByCompId(Guid compId)
        {
            return mPerformanceCycleAccess.GetPerformanceCyclesByCompId(compId);
        }

        public static int UpsertPerformanceCycles(Model.Masters.PerformanceCycle performanceCycle)
        {
            return mPerformanceCycleAccess.UpsertPerformanceCycles(performanceCycle);
        }

        #endregion


        #region Performance Goal Categories
        public static List<PMSGoalCategory> GetGoalCategoriesByCompId(Guid compId)
        {
            return mPMSGoalCategoryAccess.GetGoalCategoriesByCompId(compId);
        }

        public static PMSGoalCategory GetGoalCategoryById(int categoryId)
        {
            return mPMSGoalCategoryAccess.GetGoalCategoryById(categoryId);
        }

        public static int UpsertPMSGoalCategory(PMSGoalCategory category)
        {
            return mPMSGoalCategoryAccess.UpsertPMSGoalCategory(category);
        }

        #endregion
    }
}
