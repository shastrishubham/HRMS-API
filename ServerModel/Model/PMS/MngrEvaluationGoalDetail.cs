using ServerModel.Entity.PMS;
using System;

namespace ServerModel.Model.PMS
{
    public class MngrEvaluationGoalDetail : ManagerEvaluationDetails
    {
        public string CycleName { get; set; }
        public decimal SelfRating { get; set; }
        public string EmployeeName { get; set; }
        public string SelfEvalGoalComments { get; set; }
        public string GoalDescription { get; set; }
        public string GoalTitle { get; set; }
        public string OverallComments { get; set; }
        public Guid ManagerId { get; set; }
        public decimal MngrOverallRating { get; set; }
        public string MngrOverallComments { get; set; }
        public int MngrGoalId { get; set; }
        public decimal MngrGoalRating { get; set; }
        public string MngrGoalComment { get; set; }
    }
}
