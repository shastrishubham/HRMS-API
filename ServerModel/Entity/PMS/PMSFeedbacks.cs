using System;

namespace ServerModel.Entity.PMS
{
    public class PMSFeedbacks
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public Guid FromEmployeeId { get; set; }
        public Guid ToEmployeeId { get; set; }
        public int MS_PerformanceCycles_CycleId { get; set; }
        public string FeedbackType { get; set; }
        public string FeedbackText { get; set; }
        public decimal Rating { get; set; }
        public DateTime SubmittedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
