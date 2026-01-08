using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.BankBranchSetup
{
    public class BankBranchSetupAccess
    {
        public static List<BankBranches> GetBranchesByBankAndCompId(Guid companyId, int bankId)
        {
            List<BankBranches> bankbranches = new List<BankBranches>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetBankBranchesByBankIdAndCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", companyId);
                    command.Parameters.AddWithValue("@bankId", bankId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var branch = new BankBranches
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"]) : DateTime.Now,
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                MS_Bank_Id = reader["MS_Bank_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Bank_Id"]) : 0,
                                BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : "",
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                BranchAddress = reader["BranchAddress"] != DBNull.Value ? reader["BranchAddress"].ToString() : "",
                                BranchCode = reader["BrachCode"] != DBNull.Value ? reader["BrachCode"].ToString() : "",
                                IFSC = reader["IFSC"] != DBNull.Value ? reader["IFSC"].ToString() : "",
                                MICR = reader["MICR"] != DBNull.Value ? reader["MICR"].ToString() : "",
                                BrachContact = reader["BranchContact"] != DBNull.Value ? reader["BranchContact"].ToString() : "",
                            };
                            bankbranches.Add(branch);
                        }
                    }
                }
            }

            return bankbranches;
        }

        public static List<BankBranches> GetBankBranchesByCompId(Guid companyId)
        {
            List<BankBranches> bankbranches = new List<BankBranches>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetBankBranchesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var branch = new BankBranches
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                MS_Bank_Id = reader["MS_Bank_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Bank_Id"]) : 0,
                                BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : "",
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                BranchAddress = reader["BranchAddress"] != DBNull.Value ? reader["BranchAddress"].ToString() : "",
                                BranchCode = reader["BrachCode"] != DBNull.Value ? reader["BrachCode"].ToString() : "",
                                IFSC = reader["IFSC"] != DBNull.Value ? reader["IFSC"].ToString() : "",
                                MICR = reader["MICR"] != DBNull.Value ? reader["MICR"].ToString() : "",
                                BrachContact = reader["BranchContact"] != DBNull.Value ? reader["BranchContact"].ToString() : "",
                            };
                            bankbranches.Add(branch);
                        }
                    }
                }
            }

            return bankbranches;
        }

        public static int UpsertBankBranch(BankBranches bankBranch)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertBankBranch;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", bankBranch.Id);
                    cmd.Parameters.AddWithValue("@CompId", bankBranch.CompId);
                    cmd.Parameters.AddWithValue("@MS_Bank_Id", bankBranch.MS_Bank_Id);
                    cmd.Parameters.AddWithValue("@BranchName", bankBranch.BranchName);
                    cmd.Parameters.AddWithValue("@BranchAddress", bankBranch.BranchAddress);
                    cmd.Parameters.AddWithValue("@BrachCode", bankBranch.BranchCode);
                    cmd.Parameters.AddWithValue("@IFSC", bankBranch.IFSC);
                    cmd.Parameters.AddWithValue("@MICR", bankBranch.MICR);
                    cmd.Parameters.AddWithValue("@BranchContact", bankBranch.BrachContact);

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
