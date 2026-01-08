using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.DesignationSetup
{
    public interface IDesignationSetupAccess
    {
        int UpsertDesignation(DesignationInfo designationInfo);

        List<DesignationInfo> GetDesignationsByCompId(Guid companyId);
    }
}
