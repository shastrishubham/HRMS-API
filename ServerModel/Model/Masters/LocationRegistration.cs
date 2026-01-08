using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Masters
{
    public class LocationRegistration
    {
        public int Id { get; set; }
        public string MachineId { get; set; }
        public string MachineIp { get; set; }
        public Guid CompId { get; set; }
        public string BranchName { get; set; }
        public string MailAlias { get; set; }
        public int TimeZone_Id { get; set; }
        public string BranchCode { get; set; }
        public string BranchAddress { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string PostalCode { get; set; }
        public bool IsMainBranch { get; set; }
        public Guid BranchHead_Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool Active { get; set; }
    }
}
