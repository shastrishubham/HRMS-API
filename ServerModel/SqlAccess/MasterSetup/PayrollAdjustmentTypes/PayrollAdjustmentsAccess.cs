using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.PayrollAdjustmentTypes
{
    public class PayrollAdjustmentsAccess
    {
        public static List<PayrollAdjustmentType> GetPayrollAdjustmentTypes(Guid compId)
        {
            List<PayrollAdjustmentType> payrollAdjustmentTypes = new List<PayrollAdjustmentType>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetPayrollAdjustmentTypesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var payrollAdjustmentType = new PayrollAdjustmentType
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                IsEarningHead = Convert.ToBoolean(reader["IsEarningHead"].ToString()),
                                AdjustmentType = reader["AdjustmentType"].ToString(),
                                IsRuleBased = reader["IsRuleBased"] != DBNull.Value ? Convert.ToBoolean(reader["IsRuleBased"].ToString()) : false,
                                PercentageAmt = reader["PercentageAmt"] != DBNull.Value ?  Convert.ToDecimal(reader["PercentageAmt"].ToString()) : 0,
                                MS_SLHeads_Id = reader["MS_SLHeads_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHeads_Id"].ToString()) : 0,
                                SalaryHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : string.Empty
                            };
                            payrollAdjustmentTypes.Add(payrollAdjustmentType);
                        }
                    }
                }
            }

            return payrollAdjustmentTypes;
        }

        public static int UpsertPayrollAdjustmentType(PayrollAdjustmentType adjustmentType)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertPayrollAdjustmentType;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", adjustmentType.Id);
                    cmd.Parameters.AddWithValue("@CompId", adjustmentType.CompId);
                    cmd.Parameters.AddWithValue("@IsEarningHead", adjustmentType.IsEarningHead);
                    cmd.Parameters.AddWithValue("@AdjustmentType", adjustmentType.AdjustmentType);
                    cmd.Parameters.AddWithValue("@IsRuleBased", adjustmentType.IsRuleBased);
                    cmd.Parameters.AddWithValue("@PercentageAmt", adjustmentType.PercentageAmt ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MS_SLHeads_Id", adjustmentType.MS_SLHeads_Id ?? (object)DBNull.Value);

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
