using ServerModel.Entity.PMS;
using ServerModel.Model.Masters;

namespace ServerModel.Model.PMS
{
    public class PMSPerformanceOutcomesDetail : PMSPerformanceOutcomes
    {
        public string EmployeeName { get; set; }
        public PerformanceCycle PerformanceCycle { get; set; }
        public HRCalibrationReviewDetail HRCalibrationReviewDetail { get; set; }
        public PerformanceBand PerformanceBand { get; set; }
        public string ApproverName { get; set; }
    }
}
