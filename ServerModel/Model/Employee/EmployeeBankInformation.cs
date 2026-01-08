using System;

namespace ServerModel.Model.Employee
{
    public class EmployeeBankInformation
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid EMP_Info_Id { get; set; }
        public string FullName { get; set; }
        public int MS_Bank_Id { get; set; }
        public string BankName { get; set; }
        public int MS_Bank_Branch_Id { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string IFSC { get; set; }
        public string MICR { get; set; }
        public string IBAN { get; set; }
        public int MS_AccountType_Id { get; set; }
        public int MS_PaymentType_Id { get; set; }
        public string DDPayableAt { get; set; }
        public string NameAsPerBank { get; set; }
        public string UAN { get; set; }
        public string PFNo { get; set; }
        public bool IsCoveredESI { get; set; }
        public string ESINo { get; set; }
        public bool IsCoveredLWF { get; set; }
        public int AmendId { get; set; }
        public bool IsAmend { get; set; }
    }
}
