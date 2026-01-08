using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Model.Employee;

namespace ServerModel.SqlAccess.EmployeeSetup.AssetInfo
{
    public class EmployeeAssetAccessWrapper : IEmployeeAssetAccess
    {
        public List<EmployeeAssetInfo> GetEmployeeAssetsByCompId(Guid compId)
        {
            return EmployeeAssetAccess.GetEmployeeAssetsByCompId(compId);
        }

        public int UpsertEmployeeAssets(EmployeeAssetInfo employeeAsset)
        {
            return EmployeeAssetAccess.UpsertEmployeeAssets(employeeAsset);
        }
    }
}
