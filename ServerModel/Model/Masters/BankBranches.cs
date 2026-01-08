using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Masters
{
    public class BankBranches
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public Guid CompId { get; set; }
        public int MS_Bank_Id { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCode { get; set; }
        public string IFSC { get; set; }
        public string MICR { get; set; }
        public string BrachContact { get; set; }

    }
}
