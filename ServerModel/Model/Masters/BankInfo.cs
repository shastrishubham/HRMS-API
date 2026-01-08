using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Masters
{
    public class BankInfo
    {
        public int Id { get; set; }
        public Guid CompId { get; set; }
        public string BankName { get; set; }
        public bool Active { get; set; }
    }
}
