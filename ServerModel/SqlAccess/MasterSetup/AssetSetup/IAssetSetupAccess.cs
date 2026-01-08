using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.AssetSetup
{
    public interface IAssetSetupAccess
    {
        int UpsertAsset(AssetInfo assetInfo);

        List<AssetInfo> GetAssetsByCompId(Guid compId);
    }
}
