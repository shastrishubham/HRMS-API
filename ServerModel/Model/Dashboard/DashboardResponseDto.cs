using System.Collections.Generic;

namespace ServerModel.Model.Dashboard
{
    public class DashboardResponseDto
    {
        public DashboardSummaryDto Summary { get; set; }
        public List<BranchWiseEmployeeDto> BranchWiseEmployees { get; set; }
        public List<DesignationWiseEmployeeDto> DesignationWiseEmployees { get; set; }
        public List<DepartmentWiseEmployeeDto> DepartmentWiseEmployees { get; set; }
        public List<ShiftWiseEmployeeDto> ShiftWiseEmployees { get; set; }
        public List<PresentTodayBranchWiseDto> PresentTodayBranchWise { get; set; }
        public List<HelpDeskTicketStatusDto> HelpDeskTickets { get; set; }
        public List<InterviewVsOnboardedDto> InterviewVsOnboarded { get; set; }
    }
}
