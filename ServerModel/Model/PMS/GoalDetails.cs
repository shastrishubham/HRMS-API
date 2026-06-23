using System;

namespace ServerModel.Model.PMS
{
    public class GoalDetails : PMSEmpGoals
    {
        public string EmpId { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string BranchName { get; set; }
        public DateTime CycleStartDate { get; set; }
        public DateTime CycleEndDate { get; set; }
        public Guid ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalComment { get; set; }
    }
}
