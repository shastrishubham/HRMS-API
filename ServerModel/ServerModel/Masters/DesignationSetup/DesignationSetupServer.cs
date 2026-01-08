using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.DesignationSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.DesignationSetup
{
    public class DesignationSetupServer
    {
        #region Properties Interface

        public static IDesignationSetupAccess mDesignationSetupAccessT
            = new DesignationSetupAccessWrapper();

        #endregion


        public static List<DesignationInfo> GetDesignationsByCompId(Guid companyId)
        {
            return mDesignationSetupAccessT.GetDesignationsByCompId(companyId);
        }

        public static int UpsertDesignation(DesignationInfo designationInfo)
        {
            return mDesignationSetupAccessT.UpsertDesignation(designationInfo);
        }
    }
}
