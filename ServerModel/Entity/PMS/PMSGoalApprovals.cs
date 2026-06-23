using System;

namespace ServerModel.Entity.PMS
{
    public class PMSGoalApprovals
    {
        public int ApprovalId { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public Guid ApproverId { get; set; }
        public int MS_PerformanceCycles_Id { get; set; }
        public int MS_GoalCategories_CategoryId { get; set; }
        public int PMS_EmployeeGoals_GoalId { get; set; }
        public string ApprovalStatus { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}
