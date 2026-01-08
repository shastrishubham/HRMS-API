using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.BankSetup
{
    public class BankSetupAccess
    {
        public static List<BankInfo> GetBanksByCompId(Guid companyId)
        {
            List<BankInfo> banks = new List<BankInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetBanksDetails;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bankInfo = new BankInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                BankName = reader["BankName"].ToString(),
                                Active = Convert.ToBoolean(reader["Active"].ToString())
                            };
                            banks.Add(bankInfo);
                        }
                    }
                }
            }

            return banks;
        }

        public static int UpsertBank(BankInfo bankInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertBank;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", bankInfo.Id);
                    cmd.Parameters.AddWithValue("@CompId", bankInfo.CompId);
                    cmd.Parameters.AddWithValue("@BankName", bankInfo.BankName);
                    cmd.Parameters.AddWithValue("@Active", bankInfo.Active);

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
