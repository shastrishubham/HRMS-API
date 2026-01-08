using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Recruitment
{
    public class JobVacancyForm : Req_JbVacancy
    {
        public string DesignationName { get; set; }
        public string BranchName { get; set; }
        public string HiringManagerEmployeeName { get; set; }
    }
}
