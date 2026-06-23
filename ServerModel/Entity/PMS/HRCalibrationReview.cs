using System;

namespace ServerModel.Entity.PMS
{
    public class HRCalibrationReview
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public Guid EMP_Info_Id { get; set; }
        public int MS_PerformanceCycles_CycleId { get; set; }
        public int PMS_ManagerEvaluations_Id { get; set; }
        public Guid ReviewerId { get; set; }
        public decimal FinalRating { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Active { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
