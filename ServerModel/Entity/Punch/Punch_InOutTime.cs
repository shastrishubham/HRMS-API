using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Entity.Punch
{
    public class Punch_InOutTime
    {
        public Guid Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Date { get; set; }
        public int MS_Branch_Id { get; set; }
        public Guid EMP_Info_Id { get; set; }
        public TimeSpan Intime { get; set; }
        public TimeSpan Outtime { get; set; }
    }
}
