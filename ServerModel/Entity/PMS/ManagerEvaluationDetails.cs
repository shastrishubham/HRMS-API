using System;

namespace ServerModel.Entity.PMS
{
    public class ManagerEvaluationDetails
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public int PMS_ManagerEvaluations_Id { get; set; }
        public int PMS_EmployeeGoals_GoalId { get; set; }
        public decimal ManagerRating { get; set; }
        public string ManagerComments { get; set; }
        public bool Active { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
