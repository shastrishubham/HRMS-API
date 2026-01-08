using System;

namespace ServerModel.Model.Masters
{
    public class CompanyRegistration
    {
        public Guid Id { get; set; }
        public DateTime FormDate { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDomain { get; set; }
        public string CompanyAddress { get; set; }
        public int MS_Country_Id { get; set; }
        public int MS_State_Id { get; set; }
        public int MS_City_Id { get; set; }
        public string Pincode { get; set; }
        public string PFNo { get; set; }
        public string TANNo { get; set; }
        public string PANNo { get; set; }
        public string ESINo { get; set; }
        public string LINNo { get; set; }
        public string GSTNo { get; set; }
        public DateTime FinancialYearFrom { get; set; }
        public DateTime FinancialYearTo { get; set; }
        public string RegCertNo { get; set; }
        public int IndustryType_Id { get; set; }
        public string Website { get; set; }
        public int Timezone_Id { get; set; }
        public string ReportingCnt { get; set; }
        public string ReportingCntMail { get; set; }
        public string ReportingCntDesg { get; set; }
        public bool Active { get; set; }
    }
}
