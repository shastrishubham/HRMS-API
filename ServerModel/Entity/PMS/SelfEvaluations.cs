using ServerModel.Model.Base;
using System;

namespace ServerModel.Entity.PMS
{
    public class SelfEvaluations
    {
        public int SelfEvaluationId { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public int MS_PerformanceCycles_CycleId { get; set; }
        public Guid EMP_Info_Id { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string OverallComments { get; set; }
        public string Status { get; set; } //enum is already created - SelfEvaluationStatusTypes
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}
