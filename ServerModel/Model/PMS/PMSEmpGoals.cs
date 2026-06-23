using ServerModel.Entity.PMS;

namespace ServerModel.Model.PMS
{
    public class PMSEmpGoals : PMSEmployeeGoals
    {
        public string EmployeeName { get; set; }
        public string CycleName { get; set; }
        public string GoalCategoryName { get; set; }
    }
}
