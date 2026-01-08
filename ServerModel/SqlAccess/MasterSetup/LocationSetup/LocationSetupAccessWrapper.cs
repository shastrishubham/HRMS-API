using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.LocationSetup
{
    public class LocationSetupAccessWrapper : ILocationSetupAccess
    {
        public LocationRegistration GetLocationDetails(int locationId)
        {
            return LocationSetupAccess.GetLocationDetails(locationId);
        }

        public List<LocationRegistration> GetLocationsByCompId(Guid companyId)
        {
            return LocationSetupAccess.GetLocationsByCompId(companyId);
        }

        public int UpsertLocationSetup(LocationRegistration locationRegistration)
        {
            return LocationSetupAccess.UpsertLocationSetup(locationRegistration);
        }
    }
}
