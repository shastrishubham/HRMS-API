using System;

namespace ServerModel.Model.Masters
{
    public class DepartmentRegistration
    {
        public int Id { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentShortName { get; set; }
        public string MailAlias { get; set; }
        public Guid DepartmentLead_Id { get; set; }
        public int ParentDepartment_Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool Active { get; set; }

        public string DepartmentLeadName { get; set; }
        public string ParentDepartmentName { get; set; }
    }
}
