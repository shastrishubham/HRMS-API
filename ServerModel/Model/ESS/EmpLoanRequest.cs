using ServerModel.Database;

namespace ServerModel.Model.ESS
{
    public class EmpLoanRequest : EMP_LNReq
    {
        public string FullName { get; set; }
        public string LNTypeName { get; set; }
        public decimal InterestRate { get; set; }
    }
}
