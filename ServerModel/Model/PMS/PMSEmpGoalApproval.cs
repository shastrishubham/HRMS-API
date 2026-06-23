using ServerModel.Entity.PMS;
using System;

namespace ServerModel.Model.PMS
{
    public class PMSEmpGoalApproval : PMSGoalApprovals
    {
        public string ApproverName { get; set; }
        public string EmployeeName { get; set; }
        public string CycleName { get; set; }
        public string CategoryName { get; set; }
        public string GoalTitle { get; set; }
        public string Comment { get; set; }
        public Guid EMP_Info_Id { get; set; }
        public string GoalStatus { get; set; }
    }
}
