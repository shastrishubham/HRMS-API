using System;

namespace ServerModel.Entity.PMS
{
    public class SelfEvaluationDetails
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public int PMS_SelfEvaluations_SelfEvaluationId { get; set; }
        public int PMS_EmployeeGoals_GoalId { get; set; }
        public decimal SelfRating { get; set; }
        public string Comments { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}
