using ServerModel.Entity.PMS;
using System;

namespace ServerModel.Model.PMS
{
    public class PMSEmpCheckIns : PMSPerformanceCheckIns
    {
        public Guid EMP_Info_Id { get; set; }
        public string FullName { get; set; }
        public string GoalTitle { get; set; }
        public string GoalDescription { get; set; }
        public decimal Weightage { get; set; }
        public string TargetValue { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int MS_PerformanceCycles_Id { get; set; }
        public string CycleName { get; set; }
        public int MS_GoalCategories_CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
