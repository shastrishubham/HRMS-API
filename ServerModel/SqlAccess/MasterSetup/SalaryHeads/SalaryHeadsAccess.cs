using ServerModel.Data;
using ServerModel.Model.Employee;
using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.SalaryHeads
{
    public class SalaryHeadsAccess
    {
    }

    public static class SalaryHeadsAccess<T> where T : SalaryHeadsInfo
    {
        public static List<SalaryHeadsInfo> GetSalaryHeadsByCompId(Guid compId)
        {
            List<SalaryHeadsInfo> salaryHeads = new List<SalaryHeadsInfo>(); 
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetup.MasterSetupSqls.GetSalaryHeadsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var salaryHead = new SalaryHeadsInfo
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                SalaryHeadName = reader["SalaryHeadName"].ToString(),
                                ShortSalaryHeadName = reader["ShortSalaryHeadName"].ToString(),
                                IsEarningComponent = Convert.ToBoolean(reader["IsEarningComponent"]),
                                IsFixedComponent = Convert.ToBoolean(reader["IsFixedComponent"]),
                                IsTaxableComponent = reader["IsTaxableComponent"] != DBNull.Value ? Convert.ToBoolean(reader["IsTaxableComponent"]) : false,
                                IsShowInSalarySlip = reader["IsShowInSalarySlip"] != DBNull.Value ? Convert.ToBoolean(reader["IsShowInSalarySlip"]) : false,
                                SalaryHeadOrder = reader["SalaryHeadOrder"] != DBNull.Value ? Convert.ToInt32(reader["SalaryHeadOrder"]) : 999,
                                Percentage = reader["Percentage"] != DBNull.Value ? Convert.ToDecimal(reader["Percentage"]) : 0,
                                MS_SLHeads_Id = reader["MS_SLHeads_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHeads_Id"]) : 0,
                                HeadOf = reader["HeadOf"] != DBNull.Value ? reader["HeadOf"].ToString() : "",
                                Active = Convert.ToBoolean(reader["Active"])
                            };

                            salaryHeads.Add(salaryHead);
                        }
                    }
                }
            }

            return salaryHeads;
        }

        public static int UpsertSalaryHeads(SalaryHeadsInfo salaryHeadsInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertSalaryHeads;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", salaryHeadsInfo.Id);
                    cmd.Parameters.AddWithValue("@CompId", salaryHeadsInfo.CompId);
                    cmd.Parameters.AddWithValue("@SalaryHeadName", salaryHeadsInfo.SalaryHeadName);
                    cmd.Parameters.AddWithValue("@ShortSalaryHeadName", salaryHeadsInfo.ShortSalaryHeadName);
                    cmd.Parameters.AddWithValue("@IsEarningComponent", salaryHeadsInfo.IsEarningComponent);
                    cmd.Parameters.AddWithValue("@IsFixedComponent", salaryHeadsInfo.IsFixedComponent);
                    cmd.Parameters.AddWithValue("@IsTaxableComponent", salaryHeadsInfo.IsTaxableComponent);
                    cmd.Parameters.AddWithValue("@IsShowInSalarySlip", salaryHeadsInfo.IsShowInSalarySlip);
                    cmd.Parameters.AddWithValue("@SalaryHeadOrder", salaryHeadsInfo.SalaryHeadOrder);
                    cmd.Parameters.AddWithValue("@MS_SLHeads_Id", salaryHeadsInfo.MS_SLHeads_Id);
                    cmd.Parameters.AddWithValue("@Percentage", salaryHeadsInfo.Percentage);
                    cmd.Parameters.AddWithValue("@Active", salaryHeadsInfo.Active);

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
