using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Recruitment
{
    public class OnlineFormApplication : Req_JbForm
    {
        public string PreferredJobLocation { get; set; }
        public string DesignationName { get; set; }
        public int MS_Designation_Id { get; set; }
        public string ReferredBy { get; set; }
        public string ResumePath { get; set; }
    }
}
