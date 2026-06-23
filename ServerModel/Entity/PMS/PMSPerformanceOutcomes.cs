using System;

namespace ServerModel.Entity.PMS
{
    public class PMSPerformanceOutcomes
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public Guid EMP_Info_Id { get; set; }
        public int MS_PerformanceCycles_CycleId { get; set; }
        public int PMS_CalibrationReviews_Id { get; set; }
        public decimal FinalRating { get; set; }
        public int MS_PerformanceBand_Id { get; set; }
        public bool BonusEligible { get; set; }
        public bool PromotionRecommendation { get; set; }
        public bool PromotionEligible { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal IncrementPercentage { get; set; }
        public string DevelopmentPlan { get; set; }
        public DateTime PublishedDate { get; set; }
        public string OutcomeStatus { get; set; }
        public Guid ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public bool AppraisalLetterGenerated { get; set; }
        public DateTime AppraisalLetterGeneratedDate { get; set; }
        public DateTime PromotionEffectiveDate { get; set; }
        public DateTime IncrementEffectiveDate { get; set; }
        public string Remarks { get; set; }
        public bool IsAcknowledgedByEmployee { get; set; }
        public DateTime AcknowledgedDate { get; set; }
        public int AmmendId { get; set; }
        public bool IsAmmend { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
