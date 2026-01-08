using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model.Base;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class LeaveTypeInformationRepository
    {
        private IRespository<MS_Leave> respository = null;

        public LeaveTypeInformationRepository()
        {
            this.respository = new Repository<MS_Leave>();
        }

        public IEnumerable<LeaveInfo> GetAllLeaveTypesDetailsByCompId(Guid compId)
        {
            var leaveTypes = from a in this.respository.GetAll()
                           where a.CompId.Equals(compId) && a.Active == true
                           select new LeaveInfo
                           {
                               Id = a.Id,
                               // LeaveType = a.LeaveType,
                               //LeaveShortTypeName = a.LeaveShortTypeName,
                           };
            return leaveTypes;
        }
    }
}
