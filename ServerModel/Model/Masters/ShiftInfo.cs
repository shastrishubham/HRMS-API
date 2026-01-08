using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Masters
{
    public class ShiftInfo : MS_Shift
    {
        public List<int> WeeklyOffDays { get; set; }
        public string BranchName { get; set; }
        public string SupervisorName { get; set; }
    }
}
