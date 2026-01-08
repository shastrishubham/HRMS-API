using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.AssetSetup
{
    public class AssetSetupAccess
    {
        public static List<AssetInfo> GetAssetsByCompId(Guid compId)
        {
            List<AssetInfo> assets = new List<AssetInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetAssetsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var asset = new AssetInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                AssetName = reader["AssetName"] != DBNull.Value ? reader["AssetName"].ToString() : "",
                                IsSubmitToBack = reader["IsSubmitToBack"] != DBNull.Value ? Convert.ToBoolean(reader["IsSubmitToBack"].ToString()) : false,
                                Active = reader["Active"] != DBNull.Value ? Convert.ToBoolean(reader["Active"].ToString()) : false
                            };
                            assets.Add(asset);
                        }
                    }
                }
            }

            return assets;
        }

        public static int UpsertAsset(AssetInfo assetInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertAsset;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", assetInfo.Id);
                    cmd.Parameters.AddWithValue("@CompId", assetInfo.CompId);
                    cmd.Parameters.AddWithValue("@AssetName", assetInfo.AssetName);
                    cmd.Parameters.AddWithValue("@IsSubmitToBack", assetInfo.IsSubmitToBack);
                    cmd.Parameters.AddWithValue("@Active", assetInfo.Active);
                   
                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
