using ServerModel.Database;

namespace ServerModel.Model.ESS
{
    public class ReimbursementClaims : Reim_Claims
    {
        public string FullName { get; set; }
        public int MS_Branch_Id { get; set; }
        public string BranchName { get; set; }
        public string ReimbursementType { get; set; }
    }
}
