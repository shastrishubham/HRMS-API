using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.PMSGoalCategories
{
    public interface IPMSGoalCategoryAccess
    {
        List<PMSGoalCategory> GetGoalCategoriesByCompId(Guid compId);

        PMSGoalCategory GetGoalCategoryById(int categoryId);

        int UpsertPMSGoalCategory(PMSGoalCategory category);
    }
}
