using ServerModel.Entity.PMS;
using System;

namespace ServerModel.Model.PMS
{
    public class HRCalibrationReviewDetail : HRCalibrationReview
    {
        public string EmployeeName { get; set; }
        public string CycleName { get; set; }
        public string ReviewerName { get; set; }

        public string ManagerName { get; set; }
        public Guid ManagerId { get; set; }
    }
}
