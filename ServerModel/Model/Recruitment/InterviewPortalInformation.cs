using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Recruitment
{
    public class InterviewPortalInformation : Req_InterviewSch
    {
        public string DesignationName { get; set; }
        public string RequiredbranchName { get; set; }
        public Guid? HiringManager_Id { get; set; }
        public string HiringManagerName { get; set; }
        public Guid? ReferredById { get; set; }
        public string ReferredByName { get; set; }
        public string MyProperty { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidateContact { get; set; }
        public string Resume { get; set; }
        public Guid? InterviewTakenById { get; set; }
        public string InterviewTakenByName { get; set; }
        public string InterviewDate { get; set; }
        public string InterviewTime{ get; set; }
        public DateTime? datee { get; set; }

        public int MS_Designation_Id { get; set; }
        public decimal YearOfExp { get; set; }
        public string InterviewTakenEmp { get; set; }

       
    }
}
