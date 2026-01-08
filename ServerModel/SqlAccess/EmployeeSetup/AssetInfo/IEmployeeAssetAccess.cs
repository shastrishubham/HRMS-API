using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.AssetInfo
{
    public interface IEmployeeAssetAccess
    {
        int UpsertEmployeeAssets(EmployeeAssetInfo employeeAsset);

        List<EmployeeAssetInfo> GetEmployeeAssetsByCompId(Guid compId);

    }
}
