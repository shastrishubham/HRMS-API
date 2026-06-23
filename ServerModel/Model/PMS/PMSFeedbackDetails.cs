using ServerModel.Entity.PMS;

namespace ServerModel.Model.PMS
{
    public class PMSFeedbackDetails : PMSFeedbacks
    {
        public string FromEmployeeName { get; set; }
        public string ToEmployeeName { get; set; }
        public string CycleName { get; set; }
    }
}
