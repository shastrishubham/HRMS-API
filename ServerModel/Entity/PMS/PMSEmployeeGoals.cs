using ServerModel.Model.Base;
using System;

namespace ServerModel.Entity.PMS
{
    public class PMSEmployeeGoals
    {
        public int GoalId { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public Guid EMP_Info_Id { get; set; }
        public int MS_PerformanceCycles_Id { get; set; }
        public int MS_GoalCategories_CategoryId { get; set; }
        public string GoalTitle { get; set; }
        public string GoalDescription { get; set; }
        public decimal Weightage { get; set; }
        public string TargetValue { get; set; }
        public string AchievementCriteria { get; set; }
        public string Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn{ get; set; }
        public bool Active { get; set; }
    }
}
