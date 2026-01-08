using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.AssetInfo
{
    public class EmployeeAssetAccess
    {
        public static List<EmployeeAssetInfo> GetEmployeeAssetsByCompId(Guid compId)
        {
            throw new NotImplementedException();
        }

        public static int UpsertEmployeeAssets(EmployeeAssetInfo employeeAsset)
        {
            var jsonString1 = Newtonsoft.Json.JsonConvert.SerializeObject(employeeAsset);
            var jsonString2 = Newtonsoft.Json.JsonConvert.SerializeObject(employeeAsset.employeeAssetDetails);

            string sql = EmployeeSetup.AssetInfo.Sql.Sql.UpsertEmployeeAssetDetails;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json1", jsonString1);
                cmd.Parameters.AddWithValue("@json1", jsonString2);

                return cmd.ExecuteNonQuery();
            }
        }
    }
}
