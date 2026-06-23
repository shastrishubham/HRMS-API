using ServerModel.Entity.PMS;
using System.Collections.Generic;

namespace ServerModel.Model.PMS
{
    public class EmpSelfEvaluations : SelfEvaluations
    {
        public string CycleName { get; set; }
        public string FullName { get; set; }
        public List<SelfEvaluationDetails> SelfEvaluationDetails { get; set; }
    }
}
