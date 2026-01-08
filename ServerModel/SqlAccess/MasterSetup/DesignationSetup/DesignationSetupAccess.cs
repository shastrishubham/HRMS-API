using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.DesignationSetup
{
    public class DesignationSetupAccess
    {
        public static List<DesignationInfo> GetDesignationsByCompId(Guid companyId)
        {
            List<DesignationInfo> designations = new List<DesignationInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetDesignationsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var designationInfo = new DesignationInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                DesignationName = reader["DesignationName"].ToString(),
                                DesignationCode = reader["DesignationCode"].ToString(),
                                DesignationShortName = reader["DesignationShortName"].ToString(),
                                Active = Convert.ToBoolean(reader["Active"].ToString())
                            };
                            designations.Add(designationInfo);
                        }
                    }
                }
            }

            return designations;
        }

        public static int UpsertDesignation(DesignationInfo designationInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertDesignation;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", designationInfo.Id);
                    cmd.Parameters.AddWithValue("@formdate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@MachineIp", designationInfo.MachineIp == null ? "" : designationInfo.MachineIp);
                    cmd.Parameters.AddWithValue("@MachineId", designationInfo.MachineId == null ? "" : designationInfo.MachineId);
                    cmd.Parameters.AddWithValue("@CompId", designationInfo.CompId);
                    cmd.Parameters.AddWithValue("@DesignationName", designationInfo.DesignationName);
                    cmd.Parameters.AddWithValue("@DesignationCode", designationInfo.DesignationCode);
                    cmd.Parameters.AddWithValue("@DesignationShortName", designationInfo.DesignationShortName);
                    cmd.Parameters.AddWithValue("@Active", designationInfo.Active);

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
