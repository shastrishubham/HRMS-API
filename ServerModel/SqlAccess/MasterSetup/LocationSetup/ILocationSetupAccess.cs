using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.LocationSetup
{
    public interface ILocationSetupAccess
    {
        int UpsertLocationSetup(LocationRegistration locationRegistration);

        LocationRegistration GetLocationDetails(int locationId);

        List<LocationRegistration> GetLocationsByCompId(Guid companyId);
    }
}
