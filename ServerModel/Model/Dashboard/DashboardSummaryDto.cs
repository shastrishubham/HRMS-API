namespace ServerModel.Model.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalEmpCount { get; set; }
        public int CurrentMonthOnboardedEmpCount { get; set; }
        public int TotalBranchCount { get; set; }
        public int CurrentMonthOpenTickets { get; set; }
        public int TodaysPresentEmpCount { get; set; }
    }
}
