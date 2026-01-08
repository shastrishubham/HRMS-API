using ServerModel.Database;
using System.Collections.Generic;

namespace ServerModel.Model.HR
{
    public class LoanDisbursementInfo : LN_Disb
    {
        public List<LoanRepaymentScheduleInfo> repaymentScheduleInfos { get; set; }
        public decimal Tenure { get; set; }
    }
}
