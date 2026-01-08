using System;

namespace ServerModel.Model.Masters
{
    public class DesignationInfo
    {
        public int Id { get; set; }
        public string MachineIp { get; set; }
        public string MachineId { get; set; }
        public Guid CompId { get; set; }
        public string DesignationName { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationShortName { get; set; }
        public bool Active { get; set; }
    }
}
