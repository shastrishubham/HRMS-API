using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.PMSGoalCategories
{
    public class PMSGoalCategoryAccessWrapper : IPMSGoalCategoryAccess
    {
        public List<PMSGoalCategory> GetGoalCategoriesByCompId(Guid compId)
        {
            return PMSGoalCategoryAccess.GetGoalCategoriesByCompId(compId);
        }

        public PMSGoalCategory GetGoalCategoryById(int categoryId)
        {
            return PMSGoalCategoryAccess.GetGoalCategoryById(categoryId);
        }

        public int UpsertPMSGoalCategory(PMSGoalCategory category)
        {
            return PMSGoalCategoryAccess.UpsertPMSGoalCategory(category);   
        }
    }
}
