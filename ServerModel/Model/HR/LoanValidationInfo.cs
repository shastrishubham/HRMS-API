namespace ServerModel.Model.HR
{
    public class LoanValidationInfo
    {
        public bool IsEligible { get; set; }
        public string Reason { get; set; }
        public decimal NetSalary { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal CurrentEMI { get; set; }
        public decimal MaxAllowedEMI { get; set; }
        public decimal CalculatedEMI { get; set; }
    }
}
