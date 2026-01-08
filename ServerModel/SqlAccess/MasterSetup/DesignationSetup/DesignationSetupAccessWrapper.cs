using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.DesignationSetup
{
    public class DesignationSetupAccessWrapper : IDesignationSetupAccess
    {
        public List<DesignationInfo> GetDesignationsByCompId(Guid companyId)
        {
            return DesignationSetupAccess.GetDesignationsByCompId(companyId);
        }

        public int UpsertDesignation(DesignationInfo designationInfo)
        {
            return DesignationSetupAccess.UpsertDesignation(designationInfo);
        }
    }
}
