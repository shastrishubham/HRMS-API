using ServerModel.Entity.PMS;
using System.Collections.Generic;

namespace ServerModel.Model.PMS
{
    public class MngrEvaluation : ManagerEvaluation
    {
        public List<ManagerEvaluationDetails> ManagerEvaluationDetails { get; set; }
        public string EmpName { get; set; }
        public string ManagerName { get; set; }
        public string CycleName { get; set; }
    }
}
