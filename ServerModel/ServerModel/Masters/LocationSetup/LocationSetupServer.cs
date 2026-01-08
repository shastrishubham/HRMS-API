using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.LocationSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.LocationSetup
{
    public class LocationSetupServer
    {
        #region Properties Interface

        public static ILocationSetupAccess mLocationSetupAccessT
            = new LocationSetupAccessWrapper();

        #endregion


        public static LocationRegistration GetLocationDetails(int locationId)
        {
            return mLocationSetupAccessT.GetLocationDetails(locationId);
        }

        public static List<LocationRegistration> GetLocationsByCompId(Guid companyId)
        {
            return mLocationSetupAccessT.GetLocationsByCompId(companyId);
        }

        public static int UpsertLocationSetup(LocationRegistration locationRegistration)
        {
            return mLocationSetupAccessT.UpsertLocationSetup(locationRegistration); 
        }
    }
}
