using ServerModel.Database;
using System;

namespace ServerModel.Model.Masters
{
    public class SalaryHeadsInfo : MS_SLHeads
    {
        public int Id { get; set; }
        public Guid CompId { get; set; }
        public string SalaryHeadName { get; set; }
        public string ShortSalaryHeadName { get; set; }
        public bool IsEarningComponent { get; set; }
        public bool IsFixedComponent { get; set; }
        public bool IsTaxableComponent { get; set; }
        public int MS_SLHeads_Id { get; set; }
        public string HeadOf { get; set; }
        public decimal Percentage { get; set; }
        public bool Active { get; set; }
    }
}
