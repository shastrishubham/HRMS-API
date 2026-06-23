using ServerModel.Entity.PMS;

namespace ServerModel.Model.PMS
{
    public class EmpSelfEvaluationDetails : SelfEvaluationDetails
    {
        public int PMS_EmployeeGoals_GoalId { get; set; }
        public string CycleName { get; set; }
        public string EmployeeName { get; set; }
        public string GoalDescription { get; set; }
        public string GoalTitle { get; set; }
        public string OverallComments { get; set; }
    }
}
