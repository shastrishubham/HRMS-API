using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Model.Masters;

namespace ServerModel.SqlAccess.MasterSetup.AssetSetup
{
    public class AssetSetupAccessWrapper : IAssetSetupAccess
    {
        public List<AssetInfo> GetAssetsByCompId(Guid compId)
        {
            return AssetSetupAccess.GetAssetsByCompId(compId);
        }

        public int UpsertAsset(AssetInfo assetInfo)
        {
            return AssetSetupAccess.UpsertAsset(assetInfo);
        }
    }
}
