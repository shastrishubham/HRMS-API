using ServerModel.Database;

namespace ServerModel.Model.Training
{
    public class EmployeeTrainingInfo : EMP_Training
    {
        public string EmployeeName { get; set; }
        public string TrainingHeadEmployeeName { get; set; }
        public string TrainingName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
    }
}
