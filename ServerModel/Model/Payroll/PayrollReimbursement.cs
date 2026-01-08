using ServerModel.Database;
using System;

namespace ServerModel.Model.Payroll
{
    public class PayrollReimbursement : PR_Reimbursement
    {
        public string EmployeeName { get; set; }
        public string ApproverName { get; set; }
        public DateTime ClaimDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public Guid Approver_Emp_Id { get; set; }
        public string Approver { get; set; }
        public string ReimbursementType { get; set; }
    }
}
