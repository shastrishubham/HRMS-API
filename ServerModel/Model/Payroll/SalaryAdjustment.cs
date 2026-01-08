using ServerModel.Database;

namespace ServerModel.Model.Payroll
{
    public class SalaryAdjustment : PR_EMP_SL_Adjustment
    {
        public string FullName { get; set; }
        public string AdjustmentType { get; set; }
        public bool IsEarningHead { get; set; }

        public decimal SalaryHeadAmount { get; set; }
        public decimal CalculatedAdjustmentAmount { get; set; }
        public bool IsRuleBased { get; set; }
        public string SalaryHeadName { get; set; }

        public string Rule { get; set; }
    }
}
