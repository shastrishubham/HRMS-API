using ServerModel.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.ReimbursementTypes
{
    public class ReimbursementTypesAccess
    {
        public static List<Model.Masters.ReimbursementTypes> GetReimbursementTypesByCompId(Guid compId)
        {
            List<Model.Masters.ReimbursementTypes> reimbursementTypes = new List<Model.Masters.ReimbursementTypes>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetReimbursementTypesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reimbursementType = new Model.Masters.ReimbursementTypes
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                ReimbursementType = reader["ReimbursementType"] != DBNull.Value ? reader["ReimbursementType"].ToString() : string.Empty,
                                MonthlyLimit = reader["MonthlyLimit"] != DBNull.Value ? Convert.ToDecimal(reader["MonthlyLimit"].ToString()) : 0,
                                Frequency = reader["Frequency"] != DBNull.Value ? reader["Frequency"].ToString() : string.Empty,
                                IsDesignationSpecific = reader["IsDesignationSpecific"] != DBNull.Value ? Convert.ToBoolean(reader["IsDesignationSpecific"].ToString()) : false,
                                MS_Designation_Id = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : string.Empty
                            };
                            reimbursementTypes.Add(reimbursementType);
                        }
                    }
                }
            }

            return reimbursementTypes;
        }

        public static int UpsertReimbursementTypes(Model.Masters.ReimbursementTypes reimbursement)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertReimbursementTypes;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", reimbursement.Id);
                    cmd.Parameters.AddWithValue("@CompId", reimbursement.CompId);
                    cmd.Parameters.AddWithValue("@ReimbursementType", reimbursement.ReimbursementType);
                    cmd.Parameters.AddWithValue("@MonthlyLimit", reimbursement.MonthlyLimit);
                    cmd.Parameters.AddWithValue("@Frequency", reimbursement.Frequency);
                    cmd.Parameters.AddWithValue("@IsDesignationSpecific", (object)reimbursement.IsDesignationSpecific ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MS_Designation_Id", (object)reimbursement.MS_Designation_Id ?? DBNull.Value);

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
