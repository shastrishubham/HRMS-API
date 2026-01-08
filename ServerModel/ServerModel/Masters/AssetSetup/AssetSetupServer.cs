using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.AssetSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.AssetSetup
{
    public class AssetSetupServer
    {
        #region Properties Interface

        public static IAssetSetupAccess mAssetSetupAccessT
            = new AssetSetupAccessWrapper();

        #endregion

        public static List<AssetInfo> GetAssetsByCompId(Guid companyId)
        {
            return mAssetSetupAccessT.GetAssetsByCompId(companyId);
        }

        public static int UpsertAsset(AssetInfo assetInfo)
        {
            return mAssetSetupAccessT.UpsertAsset(assetInfo);
        }
    }
}
