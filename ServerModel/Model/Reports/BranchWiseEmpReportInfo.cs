using ServerModel.Model.Employee;
using ServerModel.Model.Masters;

namespace ServerModel.Model.Reports
{
    public class BranchWiseEmpReportInfo
    {
        public EmployeeInformation EmployeeInfo { get; set; }
        public ShiftInfo ShiftInfo { get; set; }
        public DepartmentInfo DepartmentInfo { get; set; }
        public DesignationInfo DesignationInfo { get; set; }
        public BranchInfo BranchInfo { get; set; }
    }
}
