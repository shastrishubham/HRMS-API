using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.LoanSetup
{
    public class LoanSetupAccess
    {
        public static List<LoanSetupInfo> GetLoanSetups(Guid companyId)
        {
            List<LoanSetupInfo> loanTypes = new List<LoanSetupInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetLoanTypesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var loanSetup = new LoanSetupInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                LNTypeName = reader["LNTypeName"] != DBNull.Value ? reader["LNTypeName"].ToString() : string.Empty,
                                InterestRate = reader["InterestRate"] != DBNull.Value ? Convert.ToDecimal(reader["InterestRate"].ToString()) : 0,
                                IsMaxAmtManual = reader["IsMaxAmtManual"] != DBNull.Value ? Convert.ToBoolean(reader["IsMaxAmtManual"].ToString()) : false,
                                MaxAmount = reader["MaxAmount"] != DBNull.Value ? Convert.ToDecimal(reader["MaxAmount"].ToString()) : 0,
                                MS_SLHead_Id = reader["MS_SLHead_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHead_Id"].ToString()) : 0,
                                SalaryHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : string.Empty,
                                Percentage = reader["Percentage"] != DBNull.Value ? Convert.ToDecimal(reader["Percentage"].ToString()) : 0,
                                TenureMonths = reader["TenureMonths"] != DBNull.Value ? Convert.ToDecimal(reader["TenureMonths"].ToString()) : 0,
                                AllowPartialPay = reader["AllowPartialPay"] != DBNull.Value ? Convert.ToBoolean(reader["AllowPartialPay"].ToString()) : false
                            };
                            loanTypes.Add(loanSetup);
                        }
                    }
                }
            }

            return loanTypes;
        }

        public static int UpsertLoanTypes(LoanSetupInfo loanSetupInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertLoanType;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", loanSetupInfo.Id);
                    cmd.Parameters.AddWithValue("@CompId", loanSetupInfo.CompId);
                    cmd.Parameters.AddWithValue("@LNTypeName", loanSetupInfo.LNTypeName);
                    cmd.Parameters.AddWithValue("@InterestRate", loanSetupInfo.InterestRate);
                    cmd.Parameters.AddWithValue("@IsMaxAmtManual", loanSetupInfo.IsMaxAmtManual);
                    cmd.Parameters.AddWithValue("@MaxAmount", loanSetupInfo.MaxAmount);
                    cmd.Parameters.AddWithValue("@MS_SLHead_Id", loanSetupInfo.MS_SLHead_Id);
                    cmd.Parameters.AddWithValue("@Percentage", loanSetupInfo.Percentage);
                    cmd.Parameters.AddWithValue("@TenureMonths", loanSetupInfo.TenureMonths);
                    cmd.Parameters.AddWithValue("@AllowPartialPay", loanSetupInfo.AllowPartialPay);

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
